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
    public class ChartsController : Controller
    {
        private readonly IChartsManager poolCampaignChartsManager;

        public ChartsController(IChartsManager poolCampaignChartsManager)
        {
            this.poolCampaignChartsManager = poolCampaignChartsManager;
        }

        [HttpGet]
        public IActionResult VotesDistribution(int CompanyId = 1)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = poolCampaignChartsManager.GetPieChart(CompanyId, userId);
            return View(result);
        }

        [HttpGet]
        public IActionResult VotesDistributionByTime(int CompanyId = 1)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = poolCampaignChartsManager.GetStackedChart(CompanyId, userId);
            return View(result);
        }

    }
}
