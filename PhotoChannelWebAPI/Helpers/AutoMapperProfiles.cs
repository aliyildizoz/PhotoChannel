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


            CreateMap<Channel, ChannelForDetailDto>();

            CreateMap<Photo, PhotoForListDto>();
            CreateMap<PhotoForAddDto, Photo>();
            CreateMap<Photo, PhotoForDetailDto>();
            CreateMap<Photo, PhotoGalleryDto>().ForMember(dto => dto.UserId, opt => opt.MapFrom(photo => photo.UserId))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(photo => photo.User.UserName));

            CreateMap<Photo, PhotoCardDto>().ForMember(dto => dto.UserId, opt => opt.MapFrom(photo => photo.UserId))
                .ForMember(dto => dto.UserName, opt => opt.MapFrom(photo => photo.User.UserName)).ForMember(dto => dto.ChannelId, opt => opt.MapFrom(photo => photo.ChannelId)).ForMember(dto => dto.ChannelName, opt => opt.MapFrom(photo => photo.Channel.Name)).ForMember(dto => dto.ChannelPublicId, opt => opt.MapFrom(photo => photo.Channel.PublicId)).ForMember(dto => dto.PhotoPublicId, opt => opt.MapFrom(photo => photo.PublicId));


            CreateMap<CommentForAddDto, Comment>();
            CreateMap<CommentForUpdateDto, Comment>();
            CreateMap<Comment, CommentForListDto>().ForMember(dto => dto.UserId, opt => opt.MapFrom(comment => comment.UserId)).ForMember(dto => dto.CommentId, opt => opt.MapFrom(comment => comment.Id)).ForMember(dto => dto.FirstName, opt => opt.MapFrom(comment => comment.User.FirstName)).ForMember(dto => dto.LastName, opt => opt.MapFrom(comment => comment.User.LastName));

            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailDto>();
            CreateMap<User, CurrentUserDto>();
            CreateMap<UserForUpdateDto, User>();

            CreateMap<Category, CategoryForListDto>();

            CreateMap<ChannelCategoryForAddDto, ChannelCategory>();
            CreateMap<ChannelCategoryForDeleteDto, ChannelCategory>();

            CreateMap<User, LikeForUserListDto>();

            CreateMap<User, SubscriberForListDto>();

        }

    }
}
