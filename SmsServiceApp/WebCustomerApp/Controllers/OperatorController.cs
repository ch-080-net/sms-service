using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.OperatorViewModels;
using BAL.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OperatorController : Controller
    {
        private int CurrentPage
        {
            get
            {
                return HttpContext.Session.GetInt32("CurrentPageOperator") ?? 0;
            }
            set
            {
                HttpContext.Session.SetInt32("CurrentPageOperator", value);
            }
        }

        private string SearchQuerry
        {
            get
            {
                return HttpContext.Session.GetString("SearchQuerryOperator");
            }
            set
            {
                HttpContext.Session.SetString("SearchQuerryOperator", value);
            }
        }


        private IOperatorManager operatorManager;

        public OperatorController(IOperatorManager oper)
        {
            this.operatorManager = oper;
        }

        [HttpGet]
        public IActionResult Operators()
        {
            if (this.SearchQuerry == null)
                this.SearchQuerry = "";
            ViewBag.SearchQuerry = this.SearchQuerry;
            
            ViewBag.NumOfPages = operatorManager.GetNumberOfPages(20, this.SearchQuerry);

            if (this.CurrentPage < 1)
                this.CurrentPage = 1;
            else if((ViewBag.NumOfPages - this.CurrentPage) == (-1))
                --this.CurrentPage;
            else if ((ViewBag.NumOfPages - this.CurrentPage) < (-1))
            {
                this.CurrentPage = 1;
            }
            ViewBag.CurrentPage = this.CurrentPage;

            ViewBag.Operators = operatorManager.GetPage(this.CurrentPage, 20
                , this.SearchQuerry);
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
                    return RedirectToAction("Operators", "Operator");
                }
                else
                {
                    return RedirectToAction("Operators", "Operator");
                }
            }
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult Remove(int OperatorId)
        {
            bool result = operatorManager.Remove(OperatorId);
            if (!result)
            {
                return RedirectToAction("Operators", "Operator");
            }
            else
            {
                return RedirectToAction("Operators", "Operator");
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Operator(OperatorViewModel editedOper)
        {
            if (ModelState.IsValid)
            {
                var result = operatorManager.Update(editedOper);
                if (!result)
                {
                    return RedirectToAction("Operators", "Operator");
                }
                else
                {
                    return RedirectToAction("Operators", "Operator");
                }
            }
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult NextPage()
        {
            if (this.CurrentPage < operatorManager.GetNumberOfPages(20, this.SearchQuerry))
            {
                this.CurrentPage++;
                return RedirectToAction("Operators", "Operator");
            }
            else
            {
                return RedirectToAction("Operators", "Operator");
            }
                
        }

        public IActionResult PreviousPage()
        {
            if (CurrentPage > 1)
            {
                this.CurrentPage--;
                return RedirectToAction("Operators", "Operator");
            }
            else
                return RedirectToAction("Operators", "Operator");
        }

        public IActionResult SelectPage(int Page)
        {
            this.CurrentPage = Page;
            return RedirectToAction("Operators", "Operator");         
        }


        [HttpPost]
        public IActionResult SearchOperators(OperatorSearchViewModel Search)
        {
            if (ModelState.IsValid)
            {
                this.SearchQuerry = Search.SearchQuerry ?? "";
                return RedirectToAction("Operators", "Operator");
            }
            else
            {
                return RedirectToAction("Operators", "Operator");
            }
        }

        public IActionResult EditCodes(int Id)
        {
            TempData["OperatorId"] = Id;
            return RedirectToAction("Codes", "Code");
        }
                     
    }
}
