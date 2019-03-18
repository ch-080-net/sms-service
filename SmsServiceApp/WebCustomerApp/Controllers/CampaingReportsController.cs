using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.CampaignDetailsViewModels;
using BAL.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Model.Interfaces;
using Newtonsoft.Json;
using System.Security.Claims;

namespace WebApp.Controllers
{
    [Authorize]
    public class CampaingReportsController : Controller
    {
        private readonly IChartsManager ChartsManager;

        public CampaingReportsController(IChartsManager ChartsManager)
        {
            this.ChartsManager = ChartsManager;
        }

        public IActionResult Index(int CampaignId)
        {
            if (CampaignId == 0)
                return RedirectToAction("Index", "Company");
            else
            {
                var result = new CampaignDetailsViewModel() { Selection = ChartSelection.MailingDetails,
                    CampaignId = CampaignId };
                return RedirectToAction("MailingDetails", "CampaignDetails");
            }
        }

        [HttpGet]
        public IActionResult CampaignDetails(CampaignDetailsViewModel campaignDetails)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = ChartsManager.GetChart(campaignDetails, userId);
                if (result == null)
                {
                    return RedirectToAction("Index", "Company");
                }
                return View(result);
            }
            return RedirectToAction("Index", "Company");
        }


    }
}
