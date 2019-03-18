using Microsoft.AspNetCore.Identity;
using WebCustomerApp.Models;
using Model.Interfaces;
using BAL.Managers;
using Model.ViewModels.OperatorViewModels;
using Model.ViewModels.CodeViewModels;

namespace BAL.Services
{
        public static class IdentityDataInitializer
        {
            public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
					IOperatorManager operatorManager, ICodeManager codeManager)
            {
                SeedRoles(roleManager);
                SeedUsers(userManager);
				SeedOperators(operatorManager, codeManager);
			}

            public static void SeedUsers(UserManager<ApplicationUser> userManager)
            {
                if (userManager.FindByNameAsync("User@gmail.com").Result == null)
                {
                    Phone phone = new Phone() { PhoneNumber = "+380111111111" };
                    ApplicationUser user = new ApplicationUser();
                    ApplicationGroup group = new ApplicationGroup() { Phone = phone };
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
                    ApplicationGroup group = new ApplicationGroup() { Phone = phone };
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
                    ApplicationGroup group = new ApplicationGroup() { Phone = phone };
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

			public static void SeedOperators(IOperatorManager operatorManager, ICodeManager codeManager)
			{
				if (operatorManager.GetByName("Vodafone") == null)
				{
					OperatorViewModel oper1 = new OperatorViewModel();
					oper1.Name = "Vodafone";
					operatorManager.Add(oper1);

					CodeViewModel code1 = new CodeViewModel() { OperatorId = oper1.Id, OperatorCode = "+38099" };
					CodeViewModel code2 = new CodeViewModel() { OperatorId = oper1.Id, OperatorCode = "+38066" };
					CodeViewModel code3 = new CodeViewModel() { OperatorId = oper1.Id, OperatorCode = "+38050" };
					
					codeManager.Add(code1);
					codeManager.Add(code2);
					codeManager.Add(code3);
				}

				if (operatorManager.GetByName("Kyivstar") == null)
				{
					OperatorViewModel oper2 = new OperatorViewModel();
					oper2.Name = "Kyivstar";
					operatorManager.Add(oper2);

					CodeViewModel code4 = new CodeViewModel() { OperatorId = oper2.Id, OperatorCode = "+38097" };
					CodeViewModel code5 = new CodeViewModel() { OperatorId = oper2.Id, OperatorCode = "+38067" };
					CodeViewModel code6 = new CodeViewModel() { OperatorId = oper2.Id, OperatorCode = "+38096" };
					
					codeManager.Add(code4);
					codeManager.Add(code5);
					codeManager.Add(code6);
				}
			}
        }
    }

