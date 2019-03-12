using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.CodeViewModels;
using BAL.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Model.Interfaces;
using Newtonsoft.Json;
using System.Security.Claims;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PoolCampaignChartsController : Controller
    {
        private readonly IPoolCampaignChartsManager poolCampaignChartsManager;

        public PoolCampaignChartsController(IPoolCampaignChartsManager poolCampaignChartsManager)
        {
            this.poolCampaignChartsManager = poolCampaignChartsManager;
        }

        [HttpGet]
        public IActionResult Chart(int CompanyId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Chart = poolCampaignChartsManager.GetPieChart(CompanyId, userId);
            return View();
        }
    }
}
