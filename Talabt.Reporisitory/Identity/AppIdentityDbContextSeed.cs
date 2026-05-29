using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Identity;

namespace Talabt.Reporisitory.Identity
{

    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
              if (!await roleManager.RoleExistsAsync(role))
                 await roleManager.CreateAsync(new IdentityRole(role));
        }
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Ahmed Maher",
                    Email = "Ahmedre126@gmail.com",
                    UserName = "Ahmed.BackEnd",
                    PhoneNumber = "01158238898",
                    Address = new Address
                    {
                        FirstName = "Ahmed",
                        LastName = "Maher",
                        Street = "10 Tahrir Street",
                        City = "Cairo",
                        Country = "Egypt"
                    }
                };

                await userManager.CreateAsync(user, "Password123!");
            }
        }
    }
}