using AutoMapper;
using Model.ViewModels.TariffViewModels;
using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.RecipientViewModels;
using Model.ViewModels.ContactViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.OperatorViewModels;
using Model.ViewModels.CodeViewModels;
using WebApp.Models;
using Model.ViewModels.StopWordViewModels;
using Model.ViewModels.GroupViewModels;
using Model.ViewModels.UserViewModels;
using BAL.Managers;
using Model.DTOs;
using Model.ViewModels.AnswersCodeViewModels;
using Model.ViewModels.RecievedMessageViewModel;
using System.Linq;
using Model.ViewModels.CampaignReportingViewModels;
using Model.ViewModels.EmailRecipientViewModels;
using Model.ViewModels.EmailCampaignViewModels;
using Model.ViewModels.SubscribeWordViewModels;
using Model.ViewModels.TestMessageViewModels;
using Model.ViewModels.AdminStatisticViewModel;
using MessageState = WebApp.Models.MessageState;

namespace BAL.Services
{
    /// <summary>
    ///  Mapper class for each mapping that is performed, inherited from Automapper Profile class
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Constructor with all mappings
        /// </summary>
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Company, CompanyViewModel>();
            CreateMap<CompanyViewModel, Company>();
            CreateMap<Company, ManageViewModel>();
            CreateMap<Recipient, RecipientViewModel>().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == 1 ? "Male" : "Female"))
                            .ForMember(dest => dest.Phonenumber, opt => opt.MapFrom(src => src.Phone.PhoneNumber));
            CreateMap<RecipientViewModel, Recipient>().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == "Male" ? 1 : 0));

            CreateMap<Operator, OperatorViewModel>()
                .ForMember(ovm => ovm.Logo, opt => opt.MapFrom(o => GetLogo(o)));
            CreateMap<OperatorViewModel, Operator>();


            CreateMap<Code, CodeViewModel>().ReverseMap();

            CreateMap<StopWord, StopWordViewModel>();
            CreateMap<StopWordViewModel, StopWord>();
          
            CreateMap<Tariff, TariffViewModel>();
            CreateMap<TariffViewModel, Tariff>();

            CreateMap<Contact, ContactViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == 1 ? "Male" : "Female"));
            CreateMap<ContactViewModel, Contact>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == "Male" ? 1 : 0));
            CreateMap<Code, CodeViewModel>();
            CreateMap<CodeViewModel, Code>();

            CreateMap<ApplicationGroup, GroupViewModel>().ForMember(dest => dest.ApplicationUsers, opt => opt.MapFrom(src => src.ApplicationUsers));
            CreateMap<GroupViewModel, ApplicationGroup>().ForMember(dest => dest.ApplicationUsers, opt => opt.MapFrom(src => src.ApplicationUsers));

          
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<UserViewModel, ApplicationUser>();

            CreateMap<Recipient, MessageDTO>()
                .ForMember(m => m.RecepientPhone, opt => opt.MapFrom(r => r.Phone.PhoneNumber))
                .ForMember(m => m.SenderPhone, opt => opt.MapFrom(r => r.Company.Phone.PhoneNumber))
                .ForMember(m => m.MessageText, opt => opt.MapFrom(r => ReplaceHashtags(r)))
                .ForMember(m => m.RecipientId, opt => opt.MapFrom(r => r.Id));

            CreateMap<Company, PieChart>()
                .ForMember(pc => pc.Categories, opt => opt.MapFrom(com => PopulateCategoriesForPieChart(com)))
                .ForMember(pc => pc.Description, opt => opt.MapFrom(com => com.Description));

            CreateMap<Company, CompaingPieChart>()
               .ForMember(pc => pc.Categories, opt => opt.MapFrom(com => MailingsCategoriesForPieChart(com)))
               .ForMember(pc => pc.Description, opt => opt.MapFrom(com => com.Description));

            CreateMap<Company, StackedChart>()
                .ForMember(pc => pc.TimeFrame, opt => opt.MapFrom(com => GetTimeFrameForStackedChart(com)))
                .ForMember(pc => pc.Description, opt => opt.MapFrom(com => com.Description))
                .ForMember(pc => pc.Categories, opt => opt.MapFrom(com => PopulateCategoriesForStackedChart(com)));

            CreateMap<RecievedMessage, RecievedMessageViewModel>()
                .ForMember(dest => dest.RecipientPhone, opt => opt.MapFrom(src => src.Company.Phone.PhoneNumber))
                .ForMember(dest => dest.SenderPhone, opt => opt.MapFrom(src => src.Phone.PhoneNumber))
                .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.TimeOfRecieve, opt => opt.MapFrom(src => src.RecievedTime));

            CreateMap<RecievedMessageDTO, RecievedMessage>()
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.MessageText))
                .ForMember(dest => dest.RecievedTime, opt => opt.MapFrom(src => src.TimeOfRecieve));

            CreateMap<AnswersCode, AnswersCodeViewModel>();
            CreateMap<AnswersCodeViewModel, AnswersCode>();
               
            CreateMap<PhoneGroupUnsubscribe, PhoneGroupDTO>()
                .ForMember(m => m.GroupId, opt => opt.MapFrom(r => r.GroupId))
             .ForMember(m => m.PhoneId, opt => opt.MapFrom(r => r.PhoneId));

            CreateMap<SubscribeWord, SubscribeWordViewModel>()
                .ForMember(sw => sw.Id, otp => otp.MapFrom(sw => sw.Id))
                .ForMember(sw => sw.Word, otp => otp.MapFrom(sw => sw.Word))
                .ForMember(sw => sw.CompanyId, otp => otp.MapFrom(c => c.CompanyId))
                .ForMember(sw => sw.PhoneNumber,otp => otp.MapFrom(p => p.Phone.PhoneNumber))
                .ReverseMap();

            CreateMap<Notification, EmailNotificationDTO>()
                .ForMember(en => en.Email, opt => opt.MapFrom(n => n.ApplicationUser.Email))
                .ForMember(sn => sn.Origin, opt => opt.MapFrom(n => NotificationOrigin.PersonalNotification));

            CreateMap<Notification, SmsNotificationDTO>()
                .ForMember(sn => sn.RecieverPhone, opt => opt.MapFrom(n => n.ApplicationUser.PhoneNumber))
                .ForMember(sn => sn.SenderPhone, opt => opt.MapFrom(n => n.ApplicationUser.ApplicationGroup.Phone.PhoneNumber))
                .ForMember(sn => sn.Origin, opt => opt.MapFrom(n => NotificationOrigin.PersonalNotification));

            CreateMap<SmsNotificationDTO, MessageDTO>()
                .ForMember(sn => sn.SenderPhone, opt => opt.MapFrom(n => n.SenderPhone))
                .ForMember(sn => sn.RecepientPhone, opt => opt.MapFrom(n => n.RecieverPhone))
                .ForMember(sn => sn.MessageText, opt => opt.MapFrom(n => n.Message))
                .ForMember(sn => sn.RecipientId, opt => opt.MapFrom(n => 0));

            CreateMap<Notification, WebNotificationDTO>()
                .ForMember(wn => wn.UserId, opt => opt.MapFrom(n => n.ApplicationUserId))
                .ForMember(sn => sn.Origin, opt => opt.MapFrom(n => NotificationOrigin.PersonalNotification))
                .ForMember(sn => sn.Time, opt => opt.MapFrom(n => n.Time.ToString("G")));

            CreateMap<Notification, NotificationDTO>()
                .ForMember(en => en.Origin, opt => opt.MapFrom(n => NotificationOrigin.PersonalNotification))
                .ForMember(en => en.UserId, opt => opt.MapFrom(n => n.ApplicationUserId));

            CreateMap<CampaignNotification, EmailNotificationDTO>()
                .ForMember(en => en.Email, opt => opt.MapFrom(n => n.ApplicationUser.Email))
                .ForMember(en => en.Origin, opt => opt.MapFrom(n => NotificationOrigin.CampaignReport))
                .ForMember(en => en.Title, opt => opt.MapFrom(cn => cn.Campaign.Name))
                .ForMember(en => en.Message, opt => opt.MapFrom(cn => GenerateNotificationMessage(cn)));

            CreateMap<CampaignNotification, SmsNotificationDTO>()
                .ForMember(sn => sn.RecieverPhone, opt => opt.MapFrom(n => n.ApplicationUser.PhoneNumber))
                .ForMember(sn => sn.SenderPhone, opt => opt.MapFrom(n => n.ApplicationUser.ApplicationGroup.Phone.PhoneNumber))
                .ForMember(sn => sn.Origin, opt => opt.MapFrom(n => NotificationOrigin.CampaignReport))
                .ForMember(en => en.Message, opt => opt.MapFrom(cn => GenerateNotificationMessage(cn)));

            CreateMap<CampaignNotification, WebNotificationDTO>()
                .ForMember(wn => wn.UserId, opt => opt.MapFrom(n => n.ApplicationUserId))
                .ForMember(sn => sn.Origin, opt => opt.MapFrom(n => NotificationOrigin.CampaignReport))
                .ForMember(en => en.Title, opt => opt.MapFrom(cn => cn.Campaign.Name))
                .ForMember(en => en.Message, opt => opt.MapFrom(cn => GenerateNotificationMessage(cn)))
                .ForMember(en => en.Time, opt => opt.MapFrom(cn => GetCampaignNotificationTime(cn).ToString("G")));

            CreateMap<CampaignNotification, NotificationDTO>()
                .ForMember(en => en.Origin, opt => opt.MapFrom(n => NotificationOrigin.CampaignReport))
                .ForMember(en => en.UserId, opt => opt.MapFrom(n => n.ApplicationUserId));

            CreateMap<EmailCampaignNotification, EmailNotificationDTO>()
                .ForMember(en => en.Email, opt => opt.MapFrom(n => n.EmailCampaign.User.Email))
                .ForMember(en => en.Origin, opt => opt.MapFrom(n => NotificationOrigin.EmailCampaignReport))
                .ForMember(en => en.Title, opt => opt.MapFrom(cn => cn.EmailCampaign.Name))
                .ForMember(en => en.Message, opt => opt.MapFrom(cn => GenerateNotificationMessage(cn)));

            CreateMap<EmailCampaignNotification, SmsNotificationDTO>()
                .ForMember(sn => sn.RecieverPhone, opt => opt.MapFrom(n => n.EmailCampaign.User.PhoneNumber))
                .ForMember(sn => sn.SenderPhone, opt => opt.MapFrom(n => n.EmailCampaign.User.ApplicationGroup.Phone.PhoneNumber))
                .ForMember(sn => sn.Origin, opt => opt.MapFrom(n => NotificationOrigin.EmailCampaignReport))
                .ForMember(en => en.Message, opt => opt.MapFrom(cn => GenerateNotificationMessage(cn)));

            CreateMap<EmailCampaignNotification, WebNotificationDTO>()
                .ForMember(wn => wn.UserId, opt => opt.MapFrom(n => n.EmailCampaign.UserId))
                .ForMember(sn => sn.Origin, opt => opt.MapFrom(n => NotificationOrigin.EmailCampaignReport))
                .ForMember(en => en.Title, opt => opt.MapFrom(cn => cn.EmailCampaign.Name))
                .ForMember(en => en.Message, opt => opt.MapFrom(cn => GenerateNotificationMessage(cn)))
                .ForMember(en => en.Time, opt => opt.MapFrom(cn => cn.EmailCampaign.SendingTime.ToString("G")));

            CreateMap<EmailCampaignNotification, NotificationDTO>()
                .ForMember(en => en.Origin, opt => opt.MapFrom(n => NotificationOrigin.EmailCampaignReport))
                .ForMember(en => en.UserId, opt => opt.MapFrom(n => n.EmailCampaign.UserId));

            CreateMap<EmailRecipient, EmailRecipientViewModel>()
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email.EmailAddress))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == 1 ? "Male" : "Female"));
            CreateMap<EmailRecipientViewModel, EmailRecipient>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == "Male" ? 1 : 0))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<EmailCampaign, EmailCampaignViewModel>()
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email.EmailAddress));
            CreateMap<EmailCampaignViewModel, EmailCampaign>();

            CreateMap<EmailRecipient, EmailDTO>()
                .ForMember(dest => dest.EmailRecipientId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SenderEmail, opt => opt.MapFrom(src => src.Company.Email.EmailAddress))
                .ForMember(dest => dest.RecepientEmail, opt => opt.MapFrom(src => src.Email.EmailAddress))
                .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.Company.Message));

            CreateMap<TestMessageViewModel, MessageDTO>()
                .ForMember(m => m.RecepientPhone, opt => opt.MapFrom(r => r.Recipient))
                .ForMember(m => m.SenderPhone, opt => opt.MapFrom(r => r.Sender))
                .ForMember(m => m.MessageText, opt => opt.MapFrom(r => r.Message));

            CreateMap<ApplicationGroup, AdminStatisticViewModel>()
                .ForMember(dest => dest.groupName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.groupName, opt => opt.MapFrom(cn => GetNumberOfMessage(cn)));

        }
        private int GetNumberOfMessage(ApplicationGroup applicationGroup)
        {
            int counter = 0;
            foreach (var iter in applicationGroup.Companies)
            {
                counter += iter.Recipients.Select(x => x.MessageState != MessageState.NotSent && x.MessageState != MessageState.Unsubscribed).Count();
            }

            return counter;
        }

        #region Notifications

        private DateTime GetCampaignNotificationTime(CampaignNotification cn)
        {
            switch (cn.Event)
            {
                case CampaignNotificationEvent.CampaignStart:
                    {
                        return cn.Campaign.StartTime;
                    }
                case CampaignNotificationEvent.CampaignEnd:
                    {
                        return cn.Campaign.EndTime;
                    }
                case CampaignNotificationEvent.Sending:
                    {
                        return cn.Campaign.SendingTime;
                    }
                default:
                    {
                        return DateTime.Now;
                    }
            }
        }

        private string GenerateNotificationMessage(CampaignNotification cn)
        {
            switch (cn.Event)
            {
                case CampaignNotificationEvent.CampaignStart:
                    {
                        return "Voting for campaign " + cn.Campaign.Name + " started";
                    }
                case CampaignNotificationEvent.CampaignEnd:
                    {
                        return "Voting for campaign " + cn.Campaign.Name + " ended";
                    }
                case CampaignNotificationEvent.Sending:
                    {
                        return "Mailing for campaign " + cn.Campaign.Name + " started";
                    }
                default:
                    {
                        return "";
                    }
            }            
        }

        private string GenerateNotificationMessage(EmailCampaignNotification ecn)
        {
            return "Mailing for campaign " + ecn.EmailCampaign.Name + " started";
        }

        #endregion

        /// <summary>
        /// Replaces Hashtags in message for recipient with corresponding data
        /// </summary>
        /// <param name="recipient">Should include Company entity</param>
        /// <returns>Message with replaced hashtags</returns>
        private string ReplaceHashtags(Recipient recipient)
        {
            string result = recipient.Company.Message;
            if (recipient.Name != null)
                result = result.Replace("#name", recipient.Name);
            if (recipient.Company.Name != null)
                result = result.Replace("#company", recipient.Company.Name);
            if (recipient.Surname != null)
                result = result.Replace("#surname", recipient.Surname);
            if (recipient.BirthDate != DateTime.MinValue)
                result = result.Replace("#birthday", recipient.BirthDate.ToShortDateString());

            return result;
        }

        /// <summary>
        /// Get logo path for specified Operator
        /// </summary>
        /// <returns> Path to logo for Operator or null, if not exist </returns>
        private string GetLogo(Operator oper)
        {
            string filePath = "wwwroot/images/OperatorLogo/Logo_Id=" + Convert.ToString(oper.Id) + ".png";
            if (File.Exists(filePath))
            {
                return "/images/OperatorLogo/Logo_Id=" + Convert.ToString(oper.Id) + ".png";
            }
            else
                return null;
        }

        #region Charts

        /// <summary>
        /// Get data for pie chart of specified pool campaign
        /// </summary>
        /// <returns> Neccessary data for pie chart construction </returns>
        private IEnumerable<Tuple<string, int>> PopulateCategoriesForPieChart(Company company)
        {
            var result = new List<Tuple<string, int>>();
            foreach (var code in company.AnswersCodes)
            {
                var messages = from rm in company.RecievedMessages
                               where rm.Message == Convert.ToString(code.Code)
                               select rm;
                result.Add(new Tuple<string, int>(code.Answer, messages.Count()));
            }
            return result;
        }
        private IEnumerable<Tuple<string, int>> MailingsCategoriesForPieChart(Company company)
        {
            var resultState = new List<Tuple<string, int>>();

            int accepted = company.Recipients.Count(c => c.MessageState == WebApp.Models.MessageState.Accepted);
            int delivered = company.Recipients.Count(c => c.MessageState == WebApp.Models.MessageState.Delivered);
            int notSent = company.Recipients.Count(c => c.MessageState == WebApp.Models.MessageState.NotSent);
            int reject = company.Recipients.Count(c => c.MessageState == WebApp.Models.MessageState.Rejected);
            int undeliverable = company.Recipients.Count(c => c.MessageState == WebApp.Models.MessageState.Undeliverable);
            int unsubscribed = company.Recipients.Count(c => c.MessageState == WebApp.Models.MessageState.Unsubscribed);
            

            resultState.Add(new Tuple<string, int>("Accepted", accepted));
            resultState.Add(new Tuple<string, int>("Delivered", delivered));
            resultState.Add(new Tuple<string, int>("NotSent", notSent));
            resultState.Add(new Tuple<string, int>("Rejected", reject));
            resultState.Add(new Tuple<string, int>("Undeliverable", undeliverable));
            resultState.Add(new Tuple<string, int>("Unsubscribed", unsubscribed));
           // return Json(resultState, JsonRequestBehovior.AllowGet);
            return resultState;
        }
        /// <summary>
        /// Get current time frame for specified campaign
        /// </summary>
        /// <returns> Enumeration of strings with time points for campaign </returns>
        private IEnumerable<string> GetTimeFrameForStackedChart(Company company)
        {
            DateTime beginning = company.StartTime;
            DateTime ending = (company.EndTime < DateTime.Now) ? company.EndTime : DateTime.Now;
            
            var result = new List<string>();

            foreach (var i in GetInterimTimes(beginning, ending))
            {
                result.Add(i.ToString());
            }
            return result;                
        }

        /// <summary>
        /// Produces specified quantity of interim DateTimes for specified time gates
        /// </summary>
        private IEnumerable<DateTime> GetInterimTimes(DateTime beginning, DateTime ending, int slices = 7)
        {
            if (beginning >= ending)
                return new List<DateTime>();

            if (slices < 2)
                slices = 2;
            TimeSpan span = (ending - beginning) / (slices - 1);
            var result = new List<DateTime>();
            for (int i = 0; i < slices; i++)
            {
                result.Add(beginning + span * i);
            }
            return result;
        }

        /// <summary>
        /// Get data for staced chart of specified pool campaign
        /// </summary>
        /// <returns> Neccessary data for pie chart construction </returns>
        private IEnumerable<Tuple<string, IEnumerable<int>>> PopulateCategoriesForStackedChart(Company company)
        {
            var result = new List<Tuple<string, IEnumerable<int>>>();
            var timeGates = GetInterimTimes(company.StartTime, company.EndTime);

            foreach (var code in company.AnswersCodes)
            {
                var counts = new List<int>() { 0 };
                for (int i = 0; i < timeGates.Count() - 1; i++)
                {
                    var numOfMessages = (from rm in company.RecievedMessages
                                   where rm.Message == Convert.ToString(code.Code) &&
                                   rm.RecievedTime >= timeGates.ElementAt(i) &&
                                   rm.RecievedTime < timeGates.ElementAt(i + 1)                        
                                   select rm).Count();
                    counts.Add(numOfMessages + counts.LastOrDefault());
                }
                result.Add(new Tuple<string, IEnumerable<int>>(code.Answer, counts));
            }
            return result;
        }

        #endregion

    }
}
