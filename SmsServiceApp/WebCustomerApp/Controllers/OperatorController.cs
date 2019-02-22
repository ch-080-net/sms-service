using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.ViewModels.OperatorViewModels;
using WebCustomerApp.Services;
using BAL.Managers;

namespace WebApp.Controllers
{
    

    public class OperatorController : Controller
    {
        private IOperatorManager operatorManager;

        public OperatorController(IOperatorManager oper)
        {
            this.operatorManager = oper;
        }

        [HttpGet]
        public IActionResult Operators(int Page = 1, string SearchQuerry = "")
        {
            ViewBag.Operators = operatorManager.GetPage(Page, 20, SearchQuerry);
            ViewBag.CurrentPage = Page;
            ViewBag.NumOfPages = operatorManager.GetNumberOfPages();
            ViewBag.SearchQuerry = SearchQuerry;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Operators(OperatorViewModel newOper)
        {
            if (ModelState.IsValid)
            {
                bool result = operatorManager.Add(newOper);
                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Invalid contact");
                    return RedirectToAction("Operators", "Operator");
                }
                else
                {
                    return RedirectToAction("Operators", "Operator");
                }
            }
            return View();
        }

        public IActionResult Remove(int OperatorId, int Page = 1, string SearchQuerry = "")
        {
            bool result = operatorManager.Remove(OperatorId);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Delete failed");
                return RedirectToAction("Operators", "Operator", new {SearchQuerry });
            }
            else
            {
                return RedirectToAction("Operators", "Operator", new {Page, SearchQuerry});
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Operator(OperatorViewModel editedOper)
        {
            var result = operatorManager.Update(editedOper);
            if (ModelState.IsValid)
            {
                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Modify failed");
                    return RedirectToAction("Operators", "Operator");
                }
                else
                {
                    return RedirectToAction("Operators", "Operator");
                }
            }
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult NextPage(int CurrentPage)
        {
            if (CurrentPage < operatorManager.GetNumberOfPages())
                return RedirectToAction("Operators", "Operator", new { Page = ++CurrentPage });
            else
                return RedirectToAction("Operators", "Operator", new { Page = CurrentPage });
        }

        public IActionResult PreviousPage(int CurrentPage)
        {
            if (CurrentPage > 1)
                return RedirectToAction("Operators", "Operator", new { Page = --CurrentPage });
            else
                return RedirectToAction("Operators", "Operator", new { Page = CurrentPage });
        }


        [HttpPost]
        public IActionResult SearchOperators(OperatorSearchViewModel Search)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Operators", "Operator", new {Search.SearchQuerry });
            }
            else
            {
                return RedirectToAction("Operators", "Operator");
            }
        }




    }
}
