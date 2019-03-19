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
                            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone.PhoneNumber));
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

            CreateMap<Company, StackedChart>()
                .ForMember(pc => pc.TimeFrame, opt => opt.MapFrom(com => SetTimeFrameForStackedChart(com)))
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
        }

        private string ReplaceHashtags(Recipient recipient)
        {
            string result = recipient.Company.Message;
            result = result.Replace("#name", recipient.Name)
                .Replace("#surname", recipient.Surname)
                .Replace("#company", recipient.Company.Name)
                .Replace("#birthday", recipient.BirthDate.ToShortDateString());
            return result;
        }

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

        private IEnumerable<string> SetTimeFrameForStackedChart(Company company)
        {
            DateTime beginning = company.StartTime;
            DateTime ending = (company.EndTime < DateTime.UtcNow) ? company.EndTime : DateTime.UtcNow;
            
            var result = new List<string>();

            foreach (var i in GetInterimTimes(beginning, ending))
            {
                result.Add(i.ToString());
            }
            return result;                
        }

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

    }
}
