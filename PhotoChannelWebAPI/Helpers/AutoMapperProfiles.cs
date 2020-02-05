using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.Dtos;
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
            CreateMap<User, SubscriberForListDto>();
            CreateMap<User, ChannelForAdminListDto>();

            CreateMap<Photo, PhotoForListDto>();
            CreateMap<PhotoForAddDto, Photo>();
            CreateMap<PhotoForUpdateDto, Photo>();
            CreateMap<User, LikeForUserListDto>();

            CreateMap<CommentForAddDto, Comment>();
            CreateMap<CommentForUpdateDto, Comment>();
            CreateMap<Comment, CommentForListDto>();

            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailDto>();
            CreateMap<UserForUpdateDto, User>();
        }
    }
}
