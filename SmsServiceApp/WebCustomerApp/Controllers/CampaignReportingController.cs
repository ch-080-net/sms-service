using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.CampaignReportingViewModels;
using BAL.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Model.Interfaces;
using Newtonsoft.Json;
using System.Security.Claims;

namespace WebApp.Controllers
{
    [Authorize]
    public class CampaignReportingController : Controller
    {
        private readonly IChartsManager chartsManager;

        public CampaignReportingController(IChartsManager ChartsManager)
        {
            this.chartsManager = ChartsManager;
        }

        [HttpGet]
        public IActionResult GetChart(int CampaignId)
        {
            if (CampaignId == 0)
                return NotFound();
            else
            {
                var result = new CampaignDetailsViewModel() { Selection = ChartSelection.MailingDetails,
                    CampaignId = CampaignId };
                return RedirectToAction("ShowChart", result);
            }
        }

        public IActionResult ShowChart(CampaignDetailsViewModel campaignDetails)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = chartsManager.GetChart(campaignDetails, userId);
                return View(result);
            }
            return RedirectToAction("Index", "Company");
        }

        public IActionResult Mailing(CampaignDetailsViewModel campaignDetails)
        {
            if (ModelState.IsValid)
            {
                campaignDetails.Selection = ChartSelection.MailingDetails;
                return RedirectToAction("ShowChart", "CampaignReporting", campaignDetails);
            }
            return RedirectToAction("Index", "Company");
        }

        public IActionResult Voting(CampaignDetailsViewModel campaignDetails)
        {
            if (ModelState.IsValid)
            {
                campaignDetails.Selection = ChartSelection.VotesDetails;
                return RedirectToAction("ShowChart", "CampaignReporting", campaignDetails);
            }
            return RedirectToAction("Index", "Company");
        }

        public IActionResult VotingByTime(CampaignDetailsViewModel campaignDetails)
        {
            if (ModelState.IsValid)
            {
                campaignDetails.Selection = ChartSelection.VotesDetailsByTime;
                return RedirectToAction("ShowChart", "CampaignReporting", campaignDetails);
            }
            return RedirectToAction("Index", "Company");
        }
    }
}
