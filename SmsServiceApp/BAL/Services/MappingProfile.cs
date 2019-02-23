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

namespace BAL.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Company, CompanyViewModel>();
            CreateMap<CompanyViewModel, Company>();
            CreateMap<Recipient, RecipientViewModel>();
            CreateMap<RecipientViewModel, Recipient>();

            // CreateMap<User, UserDto>();
            // CreateMap<UserDto, User>();
            CreateMap<Contact, ContactViewModel>();
            CreateMap<ContactViewModel, Contact>();
            // CreateMap<User, UserDto>();
            // CreateMap<UserDto, User>();

            CreateMap<Operator, OperatorViewModel>();
            CreateMap<OperatorViewModel, Operator>();
          
            CreateMap<Tariff, TariffViewModel>();
            CreateMap<TariffViewModel, Tariff>();
        }
    }
}
