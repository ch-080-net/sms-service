using BAL.Managers;
using Microsoft.AspNetCore.Authorization;
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
   
    [Authorize(Roles = "Admin")]
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
            return View(stopWordManager.GetStopWords());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            StopWordViewModel word = stopWordManager.GetStopWords().FirstOrDefault(c => c.Id == id);

            if (word == null)
            {
                return NotFound();
            }
            return View(word);
        }

        [HttpPost]
        [Route("~/StopWord/Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit( StopWordViewModel wordEdit)
        {
            
            if (ModelState.IsValid)
            {
                stopWordManager.Update(wordEdit);
                return RedirectToAction("Index");
            }
            return View(wordEdit);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StopWordViewModel company = stopWordManager.GetStopWords().FirstOrDefault(c => c.Id == id);

            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            stopWordManager.Delete(id);
            return RedirectToAction("Index");
        }

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
            if (ModelState.IsValid)
            {
                stopWordManager.Insert(item);
            }

            
            return RedirectToAction("Index", "StopWord");
        }

        [HttpGet]
        public IActionResult StopWordDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            StopWordViewModel word = stopWordManager.GetStopWords().FirstOrDefault(c => c.Id == id);

            if (word == null)
            {
                return NotFound();
            }
            return View(word);
        }
    }
}
