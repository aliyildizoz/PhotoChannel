using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Concrete;
using PhotoChannelWebAPI.Dtos;

namespace PhotoChannelWebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ChannelForAddDto, Channel>();
            CreateMap<ChannelForUpdateDto, Channel>();
            CreateMap<Channel, ChannelForListDto>();

            CreateMap<Photo, PhotoForListDto>();
            CreateMap<PhotoForAddDto, Photo>();
        }
    }
}
