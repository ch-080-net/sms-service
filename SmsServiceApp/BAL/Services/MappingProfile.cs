using AutoMapper;
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
            // CreateMap<User, UserDto>();
            // CreateMap<UserDto, User>();

            CreateMap<Operator, OperatorViewModel>();
            CreateMap<OperatorViewModel, Operator>();
            CreateMap<Code, CodeViewModel>();
            CreateMap<CodeViewModel, Code>();
        }
    }
}
