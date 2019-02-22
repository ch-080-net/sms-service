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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebCustomerApp.Models;
using WebCustomerApp.Models.AccountViewModels;
using WebCustomerApp.Services;
using Model.Interfaces;
using BAL.Managers;
using Model.ViewModels.OperatorViewModels;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [Route("[Controller]/[action]")]
    public class OperatorController : Controller
    {
        private IOperatorManager operatorManager;

        public OperatorController(IOperatorManager _operator)
        {
            this.operatorManager = _operator;
        }

        public IActionResult Operators()
        {
            return View();
        }

        [HttpGet]
        [Route("~/Operator/GetOperatorsCount")]
        public int GetOperatorsCount()
        {
            IEnumerable<OperatorViewModel> operators = operatorManager.GetAll();
            return operators.Count();
        }

        [HttpGet]
        [Route("~/Operator/GetOperatorsList")]
        public IEnumerable<OperatorViewModel> GetOperatorsList(int NumberOfPage)
        {
            IEnumerable<OperatorViewModel> operators = operatorManager.GetAll();
            return operators.Skip(NumberOfPage * 10 -10).Take(10).ToList();
        }

        [HttpGet("{id}")]
        public OperatorViewModel Get(int id)
        {
            var result = operatorManager.GetById(id);
            if (result != null)
                return operatorManager.GetById(id);
            else
                return new OperatorViewModel();
        }

        [HttpPost]
        [Route("/Operator/AddOperator")]
        public IActionResult AddOperator(OperatorViewModel obj)
        {
            bool result = operatorManager.Add(obj);
            if (result)
                return new ObjectResult("Operation succesfull");
            else
                return new ObjectResult("Error");
        }

        [HttpPut]
        [Route("/Operator/UpdateOperator/")]
        public IActionResult UpdateOperator(OperatorViewModel obj)
        {
            bool result = operatorManager.Update(obj);
            if (result)
                return new ObjectResult("Operation succesfull");
            else
                return new ObjectResult("Error");
        }

        [HttpDelete]
        [Route("/Phone/DeleteOperator/")]
        public IActionResult DeleteOperator(int id)
        {
            bool result = operatorManager.Remove(id);
            if (result)
                return new ObjectResult("Operation succesfull");
            else
                return new ObjectResult("Error");
        }

        [HttpGet]
        [Route("/Operator/Search/")]
        public IEnumerable<OperatorViewModel> Search(string searchData, int numberOfPage)
        {
            return operatorManager.FindByName(searchData);
        }

        [HttpGet]
        [Route("/Operator/GetNumberOfSearchOperators/")]
        public int GetNumberOfSearchOperators(string searchData)
        {
            return operatorManager.FindByName(searchData).Count();
        }
    }
}
