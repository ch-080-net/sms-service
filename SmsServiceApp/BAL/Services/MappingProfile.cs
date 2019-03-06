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
                .ForMember(m => m.MessageText, opt => opt.MapFrom(r => r.Company.Message));
        }
    }
}
