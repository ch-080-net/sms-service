using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Model.ViewModels.AdminStatisticViewModel;

namespace WebApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminStatisticController : Controller
    {
        private readonly IAdminStatisticManager adminStatisticManager;

        public AdminStatisticController(IAdminStatisticManager adminStatisticManager)
        {
            this.adminStatisticManager = adminStatisticManager;
        }
        [HttpGet]
        public IActionResult AdminStatistics(AdminStatisticViewModel adminStatisticView)
        {

            ViewBag.statistic = adminStatisticManager.NumberOfMessages();
            return View();
            
           
        }

    }
}
