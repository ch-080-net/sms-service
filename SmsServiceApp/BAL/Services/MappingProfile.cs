using AutoMapper;
using Model.ViewModels.TariffViewModels;
using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.RecipientViewModels;
using Model.ViewModels.ContactViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.OperatorViewModels;
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
            CreateMap<Recipient, RecipientViewModel>();
            CreateMap<RecipientViewModel, Recipient>();

            CreateMap<Contact, ContactViewModel>();
            CreateMap<ContactViewModel, Contact>();

            CreateMap<Operator, OperatorViewModel>();
            CreateMap<OperatorViewModel, Operator>();
           
            CreateMap<StopWord, StopWordViewModel>();
            CreateMap<StopWordViewModel, StopWord>();
          
            CreateMap<Tariff, TariffViewModel>();
            CreateMap<TariffViewModel, Tariff>();
        }
    }
}
