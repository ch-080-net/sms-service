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
using System.Security.Claims;

namespace WebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class CompanyController : Controller
    {
        private readonly ICompanyManager companyManager;

        public CompanyController(ICompanyManager company)
        {
            this.companyManager = company;
        }

        public IActionResult Index()
        {
            return View(GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [Route("~/Company/GetAll")]
        [HttpGet]
        public IEnumerable<CompanyViewModel> GetAll()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<CompanyViewModel> companies = companyManager.GetCompanies().Where(com => com.ApplicationUserId == userId);
            return companies;
        }

        [Route("~/Company/Create")]
        [HttpPost]
        public IActionResult Create(CompanyViewModel item)
        {
            string name = "{0}";

            //change according to further requirement
            item.Message = item.Message.Replace("#name", name).Replace("#company", item.Name);
            
            //then move to the send function to the SMPP 
            //foreach (var res in item.RecipientViewModels)
            //{
            //    string outServisMessage = String.Format(item.Message, RecipientViewModel.name)
            //}

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            item.ApplicationUserId = userId;
          
            companyManager.Insert(item);
            return new ObjectResult("Recipient added successfully!");
        }
    }
}
