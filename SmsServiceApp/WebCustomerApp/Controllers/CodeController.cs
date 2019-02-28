using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.CodeViewModels;
using BAL.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Model.Interfaces;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CodeController : Controller
    {
        private readonly ICodeManager codeManager;
        private readonly IOperatorManager operatorManager;

        public CodeController(ICodeManager codeManager, IOperatorManager operatorManager)
        {
            this.codeManager = codeManager;
            this.operatorManager = operatorManager;
        }

        [HttpGet]
        public IActionResult Codes(PageState pageState)
        {
            if (ModelState.IsValid)
            {
                if (TempData.ContainsKey("OperatorId"))
                    pageState.OperatorId = Convert.ToInt32(TempData["OperatorId"]);

                var page = codeManager.GetPage(pageState);

                ViewBag.Codes = page.CodeList;
                ViewBag.PageState = page.PageState;
                return View();
            }
            TempData["ErrorMessage"] = "Internal error";
            return RedirectToAction("Operators", "Operator");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Codes(CodeViewModel newCode, string pageStateJson)
        {
            if (ModelState.IsValid)
            {
                PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
                newCode.OperatorId = pageState.OperatorId;
                bool result = codeManager.Add(newCode);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Error occurred while adding code";
                    return Redirect(Url.Action("Codes", pageState));
                }
                else
                {
                    return Redirect(Url.Action("Codes", pageState));
                }
            }
            TempData["ErrorMessage"] = "Internal error";
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult Remove(int codeId, string pageStateJson)
        {
            PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
            bool result = codeManager.Remove(codeId);
            if (!result)
            {
                TempData["ErrorMessage"] = "Error occurred while removing code";
                return Redirect(Url.Action("Codes", pageState));
            }
            else
            {
                return Redirect(Url.Action("Codes", pageState));
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Code(CodeViewModel editedCode, string pageStateJson)
        {
            if (ModelState.IsValid)
            {
                PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
                editedCode.OperatorId = pageState.OperatorId;
                var result = codeManager.Update(editedCode);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Error occurred while editing code";
                    return Redirect(Url.Action("Codes", pageState));
                }
                else
                {
                    return Redirect(Url.Action("Codes", pageState));
                }
            }
            TempData["ErrorMessage"] = "Internal error";
            return RedirectToAction("Operators", "Operator");
        }

        public IActionResult NextPage(string pageStateJson)
        {
            PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
            pageState.Page++;
            return Redirect(Url.Action("Codes", pageState));
        }

        public IActionResult PreviousPage(string pageStateJson)
        {
            PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
            pageState.Page--;
            return Redirect(Url.Action("Codes", pageState));
        }

        public IActionResult SelectPage(int page, string pageStateJson)
        {
            PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
            pageState.Page = page;
            return Redirect(Url.Action("Codes", pageState));
        }

        [HttpPost]
        public IActionResult SearchCodes(CodeSearchViewModel search, string pageStateJson)
        {
            if (ModelState.IsValid)
            {
                PageState pageState = JsonConvert.DeserializeObject<PageState>(pageStateJson);
                pageState.SearchQuerry = search.SearchQuerry ?? "";
                return Redirect(Url.Action("Codes", pageState));
            }
            TempData["ErrorMessage"] = "Internal error";
            return RedirectToAction("Operators", "Operator");
        }

    }
}
