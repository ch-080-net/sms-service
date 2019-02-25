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

namespace BAL.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Company, CompanyViewModel>().ForMember(dest => dest.RecipientViewModels, opt => opt.MapFrom(src => src.Recipients));
            CreateMap<CompanyViewModel, Company>().ForMember(dest => dest.Recipients, opt => opt.MapFrom(src => src.RecipientViewModels));
            CreateMap<Recipient, RecipientViewModel>().ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone.PhoneNumber));
            CreateMap<RecipientViewModel, Recipient>();

            CreateMap<Contact, ContactViewModel>();
            CreateMap<ContactViewModel, Contact>();

            CreateMap<Operator, OperatorViewModel>();
            CreateMap<OperatorViewModel, Operator>();
          
            CreateMap<Tariff, TariffViewModel>();
            CreateMap<TariffViewModel, Tariff>();
            CreateMap<Code, CodeViewModel>();
            CreateMap<CodeViewModel, Code>();
        }
    }
}
