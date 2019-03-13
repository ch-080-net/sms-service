using AutoMapper;
using Model.ViewModels.TariffViewModels;
using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.RecipientViewModels;
using Model.ViewModels.ContactViewModels;
using System;
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

            CreateMap<Operator, OperatorViewModel>().ReverseMap();
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
                .ForMember(m => m.SenderPhone, opt => opt.MapFrom(r => r.Company.ApplicationGroup.Phone.PhoneNumber))
                .ForMember(m => m.MessageText, opt => opt.MapFrom(r => ReplaceHashtags(r)))
                .ForMember(m => m.RecipientId, opt => opt.MapFrom(r => r.Id));

            CreateMap<RecievedMessage, RecievedMessageDTO>()
                .ForMember(dest => dest.RecipientPhone, opt => opt.MapFrom(src => src.Company.Phone.PhoneNumber))
                .ForMember(dest => dest.SenderPhone, opt => opt.MapFrom(src => src.Phone.PhoneNumber))
                .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.TimeOfRecieve, opt => opt.MapFrom(src => src.RecievedTime));

            CreateMap<RecievedMessageDTO, RecievedMessage>()
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.MessageText))
                .ForMember(dest => dest.RecievedTime, opt => opt.MapFrom(src => src.TimeOfRecieve));
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
    }
}
