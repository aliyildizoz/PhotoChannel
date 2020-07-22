using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.AutoMapperConfig
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<UserForLoginDto, User>();
            CreateMap<UserForRegisterDto, User>();
        }
    }
}
