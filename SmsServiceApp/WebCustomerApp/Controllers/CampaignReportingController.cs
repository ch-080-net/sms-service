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

        /// <summary>
        /// Used for referring, redirects to ShowChart
        /// </summary>
        [HttpGet]
        public IActionResult GetChart(int campaignId)
        {
            var result = new CampaignDetailsViewModel() { Selection = ChartSelection.MailingDetails,
                CampaignId = campaignId };
            return RedirectToAction("ShowChart", result);
        }

        /// <summary>
        /// Return view with chart
        /// </summary>
        /// <param name="campaignDetails"> Should contain CampaignId </param>
        public IActionResult ShowChart(CampaignDetailsViewModel campaignDetails)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var result = chartsManager.GetChart(campaignDetails, userId);
                if (result == null)
                    return NotFound();
                return View(result);
            }
            return RedirectToAction("Index", "Company");
        }

        /// <summary>
        /// Changes ChartSelection and redirects for ShowChart()
        /// </summary>
        /// <param name="campaignDetails"></param>
        /// <returns></returns>
        public IActionResult Mailing(CampaignDetailsViewModel campaignDetails)
        {
            if (ModelState.IsValid)
            {
                campaignDetails.Selection = ChartSelection.MailingDetails;
                return RedirectToAction("ShowChart", "CampaignReporting", campaignDetails);
            }
            return RedirectToAction("Index", "Company");
        }

        /// <summary>
        /// Changes ChartSelection and redirects for ShowChart()
        /// </summary>
        /// <param name="campaignDetails"></param>
        /// <returns></returns>
        public IActionResult Voting(CampaignDetailsViewModel campaignDetails)
        {
            if (ModelState.IsValid)
            {
                campaignDetails.Selection = ChartSelection.VotesDetails;
                return RedirectToAction("ShowChart", "CampaignReporting", campaignDetails);
            }
            return RedirectToAction("Index", "Company");
        }

        /// <summary>
        /// Changes ChartSelection and redirects for ShowChart()
        /// </summary>
        /// <param name="campaignDetails"></param>
        /// <returns></returns>
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
