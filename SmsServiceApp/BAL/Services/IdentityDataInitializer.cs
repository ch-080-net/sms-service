using Microsoft.AspNetCore.Identity;
using WebCustomerApp.Models;

namespace BAL.Services
{
    public static class IdentityDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }


            public static void SeedUsers(UserManager<ApplicationUser> userManager)
            {
                if (userManager.FindByNameAsync("User@gmail.com").Result == null)
                {
                    Phone phone = new Phone() { PhoneNumber = "+380111111111" };
                    ApplicationUser user = new ApplicationUser();
                    ApplicationGroup group = new ApplicationGroup() { Phone = phone, Name = "TestUserGroup" };
                    user.UserName = "User@gmail.com";
                    user.Email = "User@gmail.com";
                    user.ApplicationGroup = group;

                IdentityResult result = userManager.CreateAsync(user, "1234ABCD").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }


                if (userManager.FindByNameAsync("Admin@gmail.com").Result == null)
                {
                    Phone phone = new Phone() { PhoneNumber = "+380777777777" };
                    ApplicationUser user = new ApplicationUser();
                    ApplicationGroup group = new ApplicationGroup() { Phone = phone, Name = "AdminGroup" };
                    user.UserName = "Admin@gmail.com";
                    user.Email = "Admin@gmail.com";
                    user.ApplicationGroup = group;

                IdentityResult result;
                result = userManager.CreateAsync(user, "1234ABCD").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

                if (userManager.FindByNameAsync("CorporateUser@gmail.com").Result == null)
                {
                    Phone phone = new Phone() { PhoneNumber = "+380666666666" };
                    ApplicationUser user = new ApplicationUser();
                    ApplicationGroup group = new ApplicationGroup() { Phone = phone, Name = "TestGroup" };
                    user.UserName = "CorporateUser@gmail.com";
                    user.Email = "CorporateUser@gmail.com";
                    user.ApplicationGroup = group;

                IdentityResult result;
                result = userManager.CreateAsync(user, "1234ABCD").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "CorporateUser").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("CorporateUser").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "CorporateUser";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
