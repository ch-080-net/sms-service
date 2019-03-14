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
using WebCustomerApp.Models;
using Model.ViewModels.StopWordViewModels;
using Model.ViewModels.GroupViewModels;
using Model.ViewModels.UserViewModels;
using BAL.Managers;
using Model.DTOs;
using System.Linq;

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
            CreateMap<Company, CompanyViewModel>().ForMember(dest => dest.RecipientViewModels, opt => opt.MapFrom(src => src.Recipients));
            CreateMap<CompanyViewModel, Company>().ForMember(dest => dest.Recipients, opt => opt.MapFrom(src => src.RecipientViewModels));
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

            CreateMap<Company, PieChartDTO>()
                .ForMember(pc => pc.Title, opt => opt.MapFrom(com => com.Name))
                .ForMember(pc => pc.Categories, opt => opt.MapFrom(com => PopulateCategories(com)))
                .ForMember(pc => pc.Description, opt => opt.MapFrom(com => com.Description));
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

        private IEnumerable<Tuple<string, int>> PopulateCategories(Company company)
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
    }
}
