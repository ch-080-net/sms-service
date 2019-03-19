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
    public class RecievedMessageService : IRecievedMessage
    {
        private readonly IStopWordManager stopWordManager;
        private readonly IRecipientManager recipientManager;
        private readonly IPhoneManager phoneManager;
        private readonly ICompanyManager companyManager;
        public RecievedMessageService(IStopWordManager stopWord , ICompanyManager companyManager , IRecipientManager recipient, IPhoneManager phoneManager)
        {
            this.phoneManager = phoneManager;
            this.recipientManager = recipient;
            this.stopWordManager = stopWord;
            this.companyManager = companyManager;
        }


        public void SearchStopWordInMeaasge(string Originator, string Destination, string Content)
        {
            Content = Content.Substring(Content.IndexOf(" Text: ") + 7);//7=" Text: " size

            var words = stopWordManager.GetStopWords().FirstOrDefault(c => c.Word == Content);//"STOP"==Content

            if (words != null)
            {/*
                Phone phoneDestination = phoneManager.GetPhones().FirstOrDefault(p => p.PhoneNumber == Destination);

                var companies = companyManager.GetCompaniesByPhone(phoneDestination);
                foreach (var company in companies)
                {
                    var recipients = recipientManager
                        .GetRecipients(company.Id)
                        .Where(r => (r.PhoneNumber==Originator)&&(r.MessageState != Model.ViewModels.RecipientViewModels.MessageState.Unsubscribed));

                    foreach (var recipient in recipients)
                    {

                        if (recipient.MessageState != Model.ViewModels.RecipientViewModels.MessageState.Unsubscribed)
                        {
                            recipient.MessageState = Model.ViewModels.RecipientViewModels.MessageState.Unsubscribed;
                            recipientManager.Update(recipient);
                        }
                    }
                }
                */
            }

        }
    }
}
