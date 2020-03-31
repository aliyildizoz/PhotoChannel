using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities.Concrete;
using Core.Utilities.Security.Jwt;
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
            CreateMap<Channel, ChannelForDetailDto>();

            CreateMap<Photo, PhotoForListDto>();
            CreateMap<PhotoForAddDto, Photo>();
            CreateMap<Photo, PhotoForDetailDto>();
            CreateMap<User, LikeForUserListDto>();

            CreateMap<CommentForAddDto, Comment>();
            CreateMap<CommentForUpdateDto, Comment>();
            CreateMap<Comment, CommentForListDto>();

            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailDto>();
            CreateMap<User, CurrentUserDto>();
            CreateMap<UserForUpdateDto, User>();

            CreateMap<Category, CategoryForListDto>();


            CreateMap<ChannelAdminForAddDto, ChannelAdmin>();
            CreateMap<ChannelAdminForDeleteDto, ChannelAdmin>();

            CreateMap<ChannelCategoryForAddDto, ChannelCategory>();
            CreateMap<ChannelCategoryForDeleteDto, ChannelCategory>();

            CreateMap<LikeForAddDto, Like>();
            CreateMap<LikeForDeleteDto, Like>();

            CreateMap<SubscriberForAddDto, Subscriber>();
            CreateMap<SubscriberForDeleteDto, Subscriber>();
        }
    }
}
