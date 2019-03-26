using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BAL.Interfaces;
using BAL.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Interfaces;
using Model.ViewModels.CompanyViewModels;
using WebApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    public class StepController : Controller
    {

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
     
     
    }
}
