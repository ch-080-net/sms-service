﻿using Microsoft.AspNetCore.Identity;
using WebApp.Models;
using Model.Interfaces;
using BAL.Managers;
using Model.ViewModels.OperatorViewModels;
using Model.ViewModels.CodeViewModels;
using Model.ViewModels.TariffViewModels;
using Model.ViewModels.StopWordViewModels;
using System.Collections.Generic;
using System.Linq;
using System;


namespace BAL.Services
{
    public static class IdentityDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
				IOperatorManager operatorManager, ICodeManager codeManager, ITariffManager tariffManager,
				IStopWordManager stopWordManager, IUnitOfWork unitOfWork)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
            SeedOperators(operatorManager, codeManager, tariffManager);
			SeedStopWords(stopWordManager);
            SeedCampaigns(unitOfWork);
            SeedEmailCampaigns(unitOfWork);

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

		public static void SeedOperators(IOperatorManager operatorManager, ICodeManager codeManager, ITariffManager tariffManager)
		{
			if (operatorManager.GetByName("Vodafone") == null)
			{
				OperatorViewModel oper1 = new OperatorViewModel();
				oper1.Name = "Vodafone";
				operatorManager.Add(oper1);
				oper1 = operatorManager.GetByName("Vodafone");

				CodeViewModel code1 = new CodeViewModel() { OperatorId = oper1.Id, OperatorCode = "+38099" };
				CodeViewModel code2 = new CodeViewModel() { OperatorId = oper1.Id, OperatorCode = "+38066" };
				CodeViewModel code3 = new CodeViewModel() { OperatorId = oper1.Id, OperatorCode = "+38050" };
					
				codeManager.Add(code1);
				codeManager.Add(code2);
				codeManager.Add(code3);

				SeedTariffs(tariffManager, oper1.Id);
			}

			if (operatorManager.GetByName("Kyivstar") == null)
			{
				OperatorViewModel oper2 = new OperatorViewModel();
				oper2.Name = "Kyivstar";
				operatorManager.Add(oper2);
				oper2 = operatorManager.GetByName("Kyivstar");

				CodeViewModel code4 = new CodeViewModel() { OperatorId = oper2.Id, OperatorCode = "+38097" };
				CodeViewModel code5 = new CodeViewModel() { OperatorId = oper2.Id, OperatorCode = "+38067" };
				CodeViewModel code6 = new CodeViewModel() { OperatorId = oper2.Id, OperatorCode = "+38096" };
					
				codeManager.Add(code4);
				codeManager.Add(code5);
				codeManager.Add(code6);

				SeedTariffs(tariffManager, oper2.Id);
			}
		}

		public static void SeedTariffs(ITariffManager tariffManager, int operatorId)
		{
			TariffViewModel tariff1 = new TariffViewModel();
			tariff1.Name = "Low";
			tariff1.Description = "Cheap tariff";
			tariff1.OperatorId = operatorId;
			tariff1.Price = 35;
			tariff1.Limit = 3;

			TariffViewModel tariff2 = new TariffViewModel();
			tariff2.Name = "Medium";
			tariff2.Description = "Better choise";
			tariff2.OperatorId = operatorId;
			tariff2.Price = 55;
			tariff2.Limit = 5;

			TariffViewModel tariff3 = new TariffViewModel();
			tariff3.Name = "High";
			tariff3.Description = "All inclusive";
			tariff3.OperatorId = operatorId;
			tariff3.Price = 80;
			tariff3.Limit = 10;

			tariffManager.Insert(tariff1);
			tariffManager.Insert(tariff2);
			tariffManager.Insert(tariff3);
		}

       
        public static void SeedStopWords(IStopWordManager stopWordManager)
		{
			IEnumerable<StopWordViewModel> stopWords = stopWordManager.GetStopWords();

			if (!stopWords.Any())
			{
				StopWordViewModel stopWord1 = new StopWordViewModel();
				StopWordViewModel stopWord2 = new StopWordViewModel();
				StopWordViewModel stopWord3 = new StopWordViewModel();

				stopWord1.Word = "START";
				stopWord2.Word = "astanavites";
				stopWord3.Word = "STOP";

				stopWordManager.Insert(stopWord1);
				stopWordManager.Insert(stopWord2);
				stopWordManager.Insert(stopWord3);
			}
		}

        public static void SeedCampaigns(IUnitOfWork unitOfWork)
        {
            if (unitOfWork.Companies.Get(com => com.Name == "Great campaign!").Any())
                return;

            var company = new Company();
            var tariff = new Tariff() { Limit = 15, Price = 10, Name = "Intermidiate", Description = "For good people!", Operator = 
                new Operator() { Name = "UMC" } };
            var phone= new Phone() { PhoneNumber = "+380333333333" };
            company.Tariff = tariff;
            company.Type = CompanyType.SendAndRecieve;
            company.StartTime = DateTime.Parse("2019.02.15");
            company.EndTime = DateTime.Parse("2019.02.21");
            company.SendingTime = DateTime.Parse("2019.02.15");
            company.Phone = phone;
            company.Name = "Great campaign!";
            company.Message = "Hello, world!";
            company.Description = "For great people only";
            company.ApplicationGroup = unitOfWork.ApplicationGroups.Get(ag => ag.Name == "AdminGroup").FirstOrDefault();

            var answerCodes = new List<AnswersCode>();
            answerCodes.Add(new AnswersCode() { Code = 0, Answer = "Yes" });
            answerCodes.Add(new AnswersCode() { Code = 1, Answer = "Of course!" });
            answerCodes.Add(new AnswersCode() { Code = 2, Answer = "Hell yeah!" });
            company.AnswersCodes = answerCodes;

            var recipients = new List<Recipient>();
            recipients.Add(new Recipient() { Phone = new Phone() { PhoneNumber = "+380673458746" }, MessageState = MessageState.NotSent });
            recipients.Add(new Recipient() { Phone = new Phone() { PhoneNumber = "+380673483746" }, MessageState = MessageState.NotSent });
            company.Recipients = recipients;

            var recievedMessages = new List<RecievedMessage>();
            recievedMessages.Add(new RecievedMessage() { Message = "1", Phone = 
                new Phone() { PhoneNumber = "+380673404646" }, RecievedTime = DateTime.Parse("2019.02.15") });
            recievedMessages.Add(new RecievedMessage() { Message = "1", Phone = 
                new Phone() { PhoneNumber = "+380683404646" }, RecievedTime = DateTime.Parse("2019.02.16") });
            recievedMessages.Add(new RecievedMessage() { Message = "1", Phone = 
                new Phone() { PhoneNumber = "+380693404646" }, RecievedTime = DateTime.Parse("2019.02.18") });

            recievedMessages.Add(new RecievedMessage() { Message = "0", Phone = 
                new Phone() { PhoneNumber = "+380073404646" }, RecievedTime = DateTime.Parse("2019.02.15") });
            recievedMessages.Add(new RecievedMessage() { Message = "0", Phone = 
                new Phone() { PhoneNumber = "+380173404646" }, RecievedTime = DateTime.Parse("2019.02.19") });

            recievedMessages.Add(new RecievedMessage() { Message = "2", Phone = 
                new Phone() { PhoneNumber = "+380673404646" }, RecievedTime = DateTime.Parse("2019.02.19") });
            recievedMessages.Add(new RecievedMessage() { Message = "2", Phone = 
                new Phone() { PhoneNumber = "+380673404646" }, RecievedTime = DateTime.Parse("2019.02.20") });

            company.RecievedMessages = recievedMessages;

            company.CompanySubscribeWords=new List<CompanySubscribeWord>()
            {
                new CompanySubscribeWord()
                {
                  SubscribeWord = new SubscribeWord(){Word="start"},
                  CompanyId = company.Id

                },
                
            };
           

            unitOfWork.Companies.Insert(company);
            unitOfWork.Save();
        }


        public static void SeedEmailCampaigns(IUnitOfWork unitOfWork)
        {
            if (unitOfWork.EmailCampaigns.Get(com => com.Name == "Great email campaign!").Any())
                return;

             var ecompany = new EmailCampaign
            {
                Email = new Email() {EmailAddress = "emailcompany@gamil.com"},
                Message = "Hello User!",
                Name = "Great email campaign!",
                Description = "example",
                SendingTime = DateTime.Now
            };



            var recipients = new List<EmailRecipient>()
          {
              new EmailRecipient(){Email = new Email(){EmailAddress = "recipient1@gamil.com"}},
              new EmailRecipient(){Email = new Email(){EmailAddress = "recipient2@gamil.com"}}
          };
           ecompany.EmailRecipients = recipients;
           ecompany.UserId = unitOfWork.ApplicationUsers.Get().First(a=>a.Email== "Admin@gmail.com").Id;
           ecompany.EmailCampaignNotifications=new List<EmailCampaignNotification>()
           {
               new EmailCampaignNotification(){
                   ApplicationUserId =ecompany.UserId,
                   BeenSent = true,
                   Type =NotificationType.Email
               }
           };
           unitOfWork.EmailCampaigns.Insert(ecompany);
           unitOfWork.Save();
        }

    }
}

