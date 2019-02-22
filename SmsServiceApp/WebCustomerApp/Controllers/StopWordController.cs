using BAL.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Interfaces;
using Model.ViewModels.StopWordViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebCustomerApp.Controllers;
using WebCustomerApp.Models;

namespace WebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class StopWordController : Controller
    {
        private readonly IStopWordManager stopWordManager;

        public StopWordController(IStopWordManager stopWord)
        {
            this.stopWordManager = stopWord;
        }

        public IActionResult Index()
        {
            IEnumerable<StopWordViewModel> stopWords = stopWordManager.GetStopWords();
            ViewBag.stopWords = stopWords;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        //[Route("~/StopWord/Edit")]
        //[HttpGet]
        //public IEnumerable<StopWordViewModel> Edit()
        //{

        //    return View();
        //}



        [Route("~/StopWord/GetAll")]
        [HttpGet]
        public IEnumerable<StopWordViewModel> GetAll()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<StopWordViewModel> companies = stopWordManager.GetStopWords();
            return companies;
        }

        [Route("~/StopWord/Create")]
        [HttpPost]
        public IActionResult Create(StopWordViewModel item)
        {
            stopWordManager.Insert(item);
            return new ObjectResult("Stop word added successfully!");
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
