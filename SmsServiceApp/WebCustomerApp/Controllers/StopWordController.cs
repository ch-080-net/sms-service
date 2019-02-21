using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Interfaces;
using Model.ViewModels.StopWordViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCustomerApp.Controllers;
using WebCustomerApp.Models;

namespace WebApp.Controllers
{
    [Route("[controller]/[action]")]
    public class StopWordController : Controller
    {
        private IUnitOfWork _unitOfWork;
        
        public StopWordController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]

        public IActionResult AddWord(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWord(StopWordDTO model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                //var word = _unitOfWork.StopWords.FirstOrDefault{w=>w.Word == model.Word };
                //if (word != null) {
                //    var word = new StopWord { Word = model.Word };
                //}
                //else
                //{
                //    // write StopWord already exists
                //}
          /*      var result = await _unitOfWork.StopWords.Create(word);

                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);*/
            }

            return View(model);
        }

      

        [HttpGet]
        public IActionResult ResetWord(string code = null)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetWord(ResetStopWordDTO model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
       /*     var word = await _unitOfWork.StopWords.SearchByWord(model.PriorWord);
            if (word == null)
            {
            var result = await _unitOfWork.StopWords.Update(word);
            }
        
            if (result.Succeeded)
            {
               return RedirectToAction(nameof(returnUrl));
            }
            AddErrors(result);*/
            return View();
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
