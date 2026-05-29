using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.APIS.Controllers;
using Talabat.APIS.DTOS;
using Talabat.APIS.Errors;
using Talabat.Core.Models.Identity;
using Talabat.Core.Service;
using Talabt.Reporisitory.Identity;

namespace Talabat.Api.Controllers
{

    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _Mapper;

        public AccountsController(UserManager<AppUser> userManager,
                    SignInManager<AppUser> signInManager,
                    ITokenService tokenService,
                    IMapper Mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _Mapper = Mapper; 
        }
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDTO registerDto)
        {
            if (CheckEmail(registerDto.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "Email already exists"));

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address == null ? null : new Address
                {
                    FirstName = registerDto.Address.FirstName,
                    LastName = registerDto.Address.LastName,
                    Street = registerDto.Address.Street,
                    City = registerDto.Address.City,
                    Country = registerDto.Address.Country
                }
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "User registration failed"));
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to assign user role"));
            var userDto = new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user, _userManager)
            };

            return Ok(userDto);
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));
            var userDto = new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user, _userManager)
            };
            return Ok(userDto);
        }
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var ReturnedObject = new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user, _userManager)
            };
            return Ok(ReturnedObject);
        }
        [HttpGet("ExistsEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
          => await _userManager.FindByEmailAsync(email) is not null;
        [Authorize]
        [HttpGet("GetAddress")]
        public async Task<ActionResult<AddressDto>> GetAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.Users
          .Include(u => u.Address)
          .FirstOrDefaultAsync(u => u.Email == email);
            var address = user.Address;
            if (address == null)
                return NotFound(new ApiResponse(404, "Address not found"));
            var addressDto = _Mapper.Map<Address, AddressDto>(address);
            return Ok(addressDto);
        }
        [Authorize]
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
        
            var user = await _userManager.Users
          .Include(u => u.Address)
          .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound(new ApiResponse(404, "User not found"));
            var MappedAddress =_Mapper.Map<AddressDto, Address>(addressDto);
             user.Address = MappedAddress;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to update address"));
            return Ok(addressDto);

        }
    }
}