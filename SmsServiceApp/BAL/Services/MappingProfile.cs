using AutoMapper;
using Model.ViewModels.CompanyViewModels;
using Model.ViewModels.RecipientViewModels;
using System;
using System.Collections.Generic;
using System.Text;
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

        }
    }
}
