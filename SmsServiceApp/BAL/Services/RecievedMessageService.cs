using AutoMapper;
using BAL.Interfaces;
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
        private readonly IPhoneGroupUManager phoneGroupUManager;

        public RecievedMessageService(IStopWordManager stopWord, IPhoneGroupUManager phoneGroupUManager)
        {
            this.phoneGroupUManager = phoneGroupUManager;
            this.stopWordManager = stopWord;
        }

        /// <summary>
        /// check who is in the reported stopword
        /// if there is something 
        /// look for the recipient and block the mailing in this group
        /// </summary>
        /// <param name="Originator">recipient PhoneNumber</param>
        /// <param name="Destination">company PhoneNumber</param>
        /// <param name="Content">message that came back</param>
        public void SearchStopWordInMeaasge(string Originator, string Destination, string Content)
        {
            Content = Content.Substring(Content.IndexOf(" Text: ") + 7);//7=" Text: " size

            var words = stopWordManager.GetStopWords().FirstOrDefault(c => c.Word == Content);//"STOP"==Content

            if (words != null)
            {
                phoneGroupUManager.AddGroupPhone(Originator, Destination);
            }

        }
    }
}
