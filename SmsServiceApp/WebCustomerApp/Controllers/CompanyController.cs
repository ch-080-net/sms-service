using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCustomerApp.Models;
using Model.ViewModels.CompanyViewModels;
using Model.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    public class CompanyController : Controller
    {
        private ICompanyManager companyManager;
        UserManager<ApplicationUser> userManager;

        public CompanyController(ICompanyManager company)
        {
            this.companyManager = company;
        }

        public IActionResult Company()
        {
            return View();
        }

       /* [Route("~/Company/GetAll")]
        [HttpGet]
        public ICollection<CompanyViewModel> GetAll()
        {
            string userId = userManager.GetUserId(User);
            List<Recipient> recipients = _unitOfWork.Recipients.Get(item => item.ApplicationUserId == userId).ToList();
            List<RecViewModel> recepientModels = new List<RecViewModel>();
            foreach (var rec in recipients)
            {
                RecViewModel recepientModel = new RecViewModel
                {
                    Id = rec.Id,
                    Name = rec.Name,
                    Address = rec.Address,
                    PhoneNumber = rec.PhoneNumber,
                    Gender = rec.Gender,
                    ApplicationUserId = rec.ApplicationUserId
                };
                recepientModels.Add(recepientModel);
            }
            return recepientModels;
        }*/
    }
}
