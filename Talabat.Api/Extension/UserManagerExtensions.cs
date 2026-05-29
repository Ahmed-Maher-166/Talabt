using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Models.Identity;
namespace Talabat.APIS.Extension
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByEmailWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == Email);
              return user;
        }
    }
}
