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

namespace WebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class OperatorController
    {
        [HttpGet]
        [Route("~/Operator/GetOperatorsCount")]
        public int GetOperatorsCount()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("~/Operator/GetOperatorsList")]
        public int GetOperatorsList()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("/Operator/AddOperator")]
        public int AddOperator()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("/Operator/UpdateOperator/")]
        public int UpdateOperator()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("/Phone/DeleteOperator/")]
        public int DeleteOperator()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("/Operator/Search/")]
        public int Search()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("/Operator/GetNumberOfSearchOperators/")]
        public int GetNumberOfSearchOperators()
        {
            throw new NotImplementedException();
        }


    }
}
