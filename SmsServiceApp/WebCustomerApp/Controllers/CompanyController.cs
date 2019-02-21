using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCustomerApp.Models;

namespace WebApp.Controllers
{
    public class CompanyController : Controller
    {
        private ICompanyManager companyManager;

        public CompanyController(ICompanyManager company)
        {
            this.companyManager = company;
        }

        public IEnumerable<Company> GetAll()
        {
            return companyManager.GetCompanies();
        }
    }
}
