using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.OperatorViewModels;
using BAL.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using WebCustomerApp.Models;
using Model.Interfaces;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OperatorController : Controller
    {

        private IOperatorManager operatorManager;

        public OperatorController(IOperatorManager oper)
        {
            this.operatorManager = oper;
        }
        
        [HttpGet]
        public IActionResult Operators(PageState pageState)
        {
            if (ModelState.IsValid)
            {
                var page = operatorManager.GetPage(pageState);

                ViewBag.Operators = page.OperatorList;
                ViewBag.PageState = page.PageState;
                return View();
            }
            TempData["ErrorMessage"] = "Internal error";
            return RedirectToAction("Operators", "Operator");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Operators(OperatorViewModel newOper, string pageStateJson)
        {
            if (ModelState.IsValid)
            {
                PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
                bool result = operatorManager.Add(newOper);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Error occured while adding new operator";
                }
                else
                {
                    return Redirect(Url.Action("Operators", pageState));
                }
            }
            TempData["ErrorMessage"] = "Internal error!";
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult Remove(int operatorId, string pageStateJson)
        {
            PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
            bool result = operatorManager.Remove(operatorId);
            if (!result)
            {
                TempData["ErrorMessage"] = "Error occured while removing operator";
                return Redirect(Url.Action("Operators", pageState));
            }
            else
            {
                return Redirect(Url.Action("Operators", pageState));
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Operator(OperatorViewModel editedOper, string pageStateJson)
        {
            if (ModelState.IsValid)
            {
                PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
                var result = operatorManager.Update(editedOper);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Error occured while editing operator";
                    return Redirect(Url.Action("Operators", pageState));
                }
                else
                {
                    return Redirect(Url.Action("Operators", pageState));
                }
            }
            TempData["ErrorMessage"] = "Internal error!";
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult NextPage(string pageStateJson)
        {
            PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
            pageState.Page++;
            return Redirect(Url.Action("Operators", pageState));
        }

        public IActionResult PreviousPage(string pageStateJson)
        {
            PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
            pageState.Page--;
            return Redirect(Url.Action("Operators", pageState));
        }

        public IActionResult SelectPage(int page, string pageStateJson)
        {
            PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
            pageState.Page = page;
            return Redirect(Url.Action("Operators", pageState));
        }

        [HttpPost]
        public IActionResult SearchOperators(OperatorSearchViewModel search, string pageStateJson)
        {
            if (ModelState.IsValid)
            {
                PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
                pageState.SearchQuerry = search.SearchQuerry ?? "";
                return Redirect(Url.Action("Operators", pageState));
            }
            TempData["ErrorMessage"] = "Internal error";
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult EditCodes(int id)
        {
            TempData["OperatorId"] = id;
            return RedirectToAction("Operators", "Operator");
        }

        [HttpGet]
        public IActionResult AddLogo(int operatorId)
        {
            return View(new LogoViewModel() { OperatorId = operatorId });
        }

        [HttpPost]
        public IActionResult AddLogo(LogoViewModel logo)
        {
            operatorManager.AddLogo(logo);
            return RedirectToAction("Operators", "Operator");
        }

    }
}
