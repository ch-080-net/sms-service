using AutoMapper;
using BAL.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Model.ViewModels.GroupViewModels;
using Model.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize(Roles = "CorporateUser")]
    public class GroupController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IGroupManager groupManager;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;
        private readonly INotificationManager notificationManager;
       

        public GroupController(UserManager<ApplicationUser> userManager, IMapper mapper
            , IGroupManager groupManager, IEmailSender emailSender, NotificationManager notificationManager)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.groupManager = groupManager;
            this.emailSender = emailSender;
            this.notificationManager = notificationManager;
        }

        /// <summary>
        /// Gets View of users that in current user ApplicationGroup
        /// </summary>
        /// <returns>List of Users ViewModels</returns>
        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = userManager.Users.FirstOrDefault(u => u.Id == userId);
            var groupId = user.ApplicationGroupId;
            IEnumerable<ApplicationUser> users = userManager.Users.Where(u => u.ApplicationGroupId == groupId).ToList();
            var userModels = mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserViewModel>>(users);
            foreach(var u in userModels)
            {
                u.GroupName = groupManager.Get(groupId).Name;
            }
            return View(userModels);
        }

        /// <summary>
        /// Gets View for sending invite to current ApplicationGroup
        /// </summary>
        /// <returns>View for sending invitings</returns>
        [HttpGet]
        public IActionResult AddUsers()
        {
            return View();
        }

        /// <summary>
        /// Takes email from view and send inviting to user, if user not exist - send a link to registration whith group id.
        /// If user exist - add to his property invite group id, and he has a link to his profile
        /// </summary>
        /// <param name="item">Info from view</param>
        /// <returns>View with group members</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUsers([Bind] GroupUserViewModel item)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userThis = userManager.Users.FirstOrDefault(u => u.Id == userId);
                var groupId = userThis.ApplicationGroupId;
                var groupName = groupManager.Get(groupId).Name;
                var user = userManager.Users.FirstOrDefault(u => u.Email == item.Email);
                if(user == null)
                {
                    string subjectNew = "Invite to SMS Service registration";
                    var sb = new StringBuilder("You invited to SMS Servise Application by ");
                    sb.AppendFormat("{0} company. If you know this company - please ", groupName);
                    string dom = "http://localhost:53759";
                    sb.AppendFormat("<a href='{0}/Account/NewRegister?groupId={1}'> click here to registration </a>", dom, groupId);
                    string messageNew = sb.ToString();
                    emailSender.SendEmailAsync(item.Email, subjectNew, messageNew);
                }
                else
                {
                    user.InviteId = groupId;
                    notificationManager.AddNotificationsToUser(userId, DateTime.Now, "Group invite", "You have been invited to group");
                    userManager.UpdateAsync(user);
                    string subjectNew = "SMS Service invite you to join the group";
                    var sb = new StringBuilder("You invited to join the group ");
                    sb.AppendFormat("{0}. If you accept this - please ", groupName);
                    string dom = "http://localhost:53759";
                    sb.AppendFormat("<a href='{0}/Manage/Index'> click here to join </a>", dom);
                    string messageNew = sb.ToString();
                    emailSender.SendEmailAsync(item.Email, subjectNew, messageNew);
                }
                return RedirectToAction("Index");
            }
            return View(item);
        }

        /// <summary>
        /// Delete user from Application
        /// </summary>
        /// <param name="id">Id of User which need to delete</param>
        /// <returns>View with group users</returns>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            if(id == this.User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                ModelState.AddModelError(string.Empty, "You can't delete yourself.");
                ViewData["Delete"] = "false";
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(mapper.Map<ApplicationUser, UserViewModel>(user));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            await userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
