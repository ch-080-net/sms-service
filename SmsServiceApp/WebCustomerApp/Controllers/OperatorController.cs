using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.OperatorViewModels;
using BAL.Managers;

namespace WebApp.Controllers
{
    // Make Model errors passing via TempData and AddModelError? Same for pagination and search?
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
            ViewBag.NumOfPages = operatorManager.GetNumberOfPages(20, SearchQuerry);
            ViewBag.CurrentPage = (Page <= ViewBag.NumOfPages)? Page : --Page;
            ViewBag.SearchQuerry = SearchQuerry;

            var operators = operatorManager.GetPage(Page, 20, SearchQuerry);
            var navoperators = new List<OperatorWithNavigationViewModel>();
            foreach (var iter in operators)
            {
                navoperators.Add(new OperatorWithNavigationViewModel()
                {
                    Operator = iter,
                    Page = Page,
                    SearchQuerry = SearchQuerry
                });
            }
            ViewBag.Operators = navoperators;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Operators(OperatorWithNavigationViewModel newOper)
        {
            if (ModelState.IsValid)
            {
                bool result = operatorManager.Add(newOper.Operator);
                if (!result)
                {
                    return RedirectToAction("Operators", "Operator");
                }
                else
                {
                    return RedirectToAction("Operators", "Operator", new { newOper.Page, newOper.SearchQuerry });
                }
            }
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult Remove(int OperatorId, int Page = 1, string SearchQuerry = "")
        {
            bool result = operatorManager.Remove(OperatorId);
            if (!result)
            {
                return RedirectToAction("Operators", "Operator");
            }
            else
            {
                return RedirectToAction("Operators", "Operator", new {Page, SearchQuerry});
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Operator(OperatorWithNavigationViewModel editedOper)
        {
            if (ModelState.IsValid)
            {
                var result = operatorManager.Update(editedOper.Operator);
                if (!result)
                {
                    return RedirectToAction("Operators", "Operator");
                }
                else
                {
                    return RedirectToAction("Operators", "Operator", new { editedOper.Page, editedOper.SearchQuerry });
                }
            }
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult NextPage(int CurrentPage, string SearchQuerry = "")
        {
            if (CurrentPage < operatorManager.GetNumberOfPages())
                return RedirectToAction("Operators", "Operator", new { Page = ++CurrentPage, SearchQuerry });
            else
                return RedirectToAction("Operators", "Operator", new { Page = CurrentPage, SearchQuerry });
        }

        public IActionResult PreviousPage(int CurrentPage, string SearchQuerry = "")
        {
            if (CurrentPage > 1)
                return RedirectToAction("Operators", "Operator", new { Page = --CurrentPage, SearchQuerry });
            else
                return RedirectToAction("Operators", "Operator", new { Page = CurrentPage, SearchQuerry });
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
