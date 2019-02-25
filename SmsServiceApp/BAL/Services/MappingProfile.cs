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

namespace BAL.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Company, CompanyViewModel>().ForMember(dest => dest.RecipientViewModels, opt => opt.MapFrom(src => src.Recipients));
            CreateMap<CompanyViewModel, Company>().ForMember(dest => dest.Recipients, opt => opt.MapFrom(src => src.RecipientViewModels));
            CreateMap<Recipient, RecipientViewModel>().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == 1 ? "Male" : "Female"))
                            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone.PhoneNumber));
            CreateMap<RecipientViewModel, Recipient>().ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == "Male" ? 1 : 0));

            CreateMap<Operator, OperatorViewModel>();
            CreateMap<OperatorViewModel, Operator>();
           
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
        }
    }
}
