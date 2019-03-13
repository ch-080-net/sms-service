using AutoMapper;
using BAL.Managers;
using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCustomerApp.Models;

namespace BAL.Services
{
    public class ResivedMessage :IResivedMessage
    {
        private readonly IStopWordManager stopWordManager;
        private readonly IRecipientManager recipientManager;
        private readonly ICompanyManager companyManager;
        private readonly IGroupManager groupManager;

        public ResivedMessage(IStopWordManager stopWord ,IRecipientManager recipient, IGroupManager groupManager,ICompanyManager companyManager)
        {
            this.companyManager = companyManager;
            this.groupManager = groupManager;
            this.recipientManager = recipient;
            this.stopWordManager = stopWord;
        }


        public void SearchStopWordInMeaasge(string Originator, string Destination, string Content)
        {
            Content = Content.Substring(Content.IndexOf(" Text: ") + 7);//7=" Text: " size

            var words = stopWordManager.GetStopWords().FirstOrDefault(c => c.Word == Content);
            if (words != null)
            {

                //var group=groupManager.GetGroups().FirstOrDefault(g=>g.)
                //var company = companyManager.GetCompanies().FirstOrDefault((r=>r.Company.Phone.PhoneNumber == Destination));
                //    var recipient = recipientManager.GetRecipients().FirstOrDefault(r => (r.Phone.PhoneNumber == Originator) &&);
                //if (recipient != null)
                //{
                //    recipient.IsStopped = true;
                //    stopWordManager.Recipients.Update(recipient);
                //}
            }
        }
    }
}
