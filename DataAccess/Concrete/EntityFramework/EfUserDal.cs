using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, PhotoChannelContext>, IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.UserOperationClaims
                    .Where(userOperationClaim => userOperationClaim.UserId == user.Id).Join(context.OperationClaims,
                        userOperationClaim => userOperationClaim.OperationClaimId,
                        operationClaimId => operationClaimId.Id,
                        (userOperationClaim, operationClaim) => operationClaim);
                return result.ToList();
            }
        }

        public List<Channel> GetSubscriptionList(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.Subscribers.Where(subscriber => subscriber.UserId == user.Id).Join(
                    context.Channels, subscriber => subscriber.ChannelId, channel => channel.Id,
                    (subscriber, channel) => channel);
                return result.ToList();
            }
        }

        public List<Channel> GetChannels(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.Channels.Where(channel => channel.OwnerId == user.Id);
                return result.ToList();
            }
        }

        public List<PhotoCardDto> GetLikedPhotos(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var likedPhotos = context.Likes.Where(like => like.UserId == user.Id).Join(
                    context.Photos, like => like.PhotoId, photo => photo.Id,
                    (like, photo) => new PhotoCardDto
                    {
                        Photo = photo
                    }).ToList();

                likedPhotos.ForEach(dto =>
                {
                    dto.Likes = context.Likes.Where(like => like.PhotoId == dto.Photo.Id).Join(context.Users,
                        like => like.UserId, u => u.Id, (like, u) => new LikeForUserListDto
                        {
                            UserId = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName
                        }).ToList();
                    dto.Comments = context.Comments.Where(comment => comment.PhotoId == dto.Photo.Id).Select(comment =>
                        new CommentForListDto
                        {
                            LastName = context.Users.FirstOrDefault(u => u.Id == comment.UserId).LastName,
                            FirstName = context.Users.FirstOrDefault(u => u.Id == comment.UserId).FirstName,
                            Description = comment.Description,
                            UserId = comment.UserId,
                            ShareDate = comment.ShareDate,
                            CommentId = comment.Id
                        }).ToList();

                });
                return likedPhotos;
            }
        }

        public List<PhotoCardDto> GetSharedPhotos(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var likedPhotos = context.Photos.Where(like => like.UserId == user.Id).Select(photo => new PhotoCardDto
                {
                    Photo = photo
                }).ToList();

                likedPhotos.ForEach(dto =>
                {
                    dto.Likes = context.Likes.Where(like => like.PhotoId == dto.Photo.Id).Join(context.Users,
                        like => like.UserId, u => u.Id, (like, u) => new LikeForUserListDto
                        {
                            UserId = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName
                        }).ToList();
                    dto.Comments = context.Comments.Where(comment => comment.PhotoId == dto.Photo.Id).Select(comment =>
                        new CommentForListDto
                        {
                            LastName = context.Users.FirstOrDefault(u => u.Id == comment.UserId).LastName,
                            FirstName = context.Users.FirstOrDefault(u => u.Id == comment.UserId).FirstName,
                            Description = comment.Description,
                            UserId = comment.UserId,
                            ShareDate = comment.ShareDate,
                            CommentId = comment.Id
                        }).ToList();
                    dto.ChannelName = context.Channels.FirstOrDefault(channel => channel.Id == dto.Photo.ChannelId)
                        .Name;
                    dto.UserName = context.Users.FirstOrDefault(u => u.Id == dto.Photo.UserId).UserName;
                });
                return likedPhotos;
            }
        }

        public void AddOperationClaim(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                context.UserOperationClaims.Add(new UserOperationClaim
                {
                    UserId = user.Id,
                    OperationClaimId = 2
                });
                context.SaveChanges();
            }
        }
    }
}
