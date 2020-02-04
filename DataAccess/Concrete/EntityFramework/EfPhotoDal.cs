using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.Extensions.Logging;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfPhotoDal : EfEntityRepositoryBase<Photo, PhotoChannelContext>, IPhotoDal
    {
        public List<User> GetLikeUsersByPhoto(Photo photo)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.Likes.Where(like => like.PhotoId == photo.Id).Join(context.Users,
                    like => like.UserId, user => user.Id, (like, user) => user);
                return result.ToList();
            }
        }

        public void RelatedDelete(Photo photo)
        {
            using (var context = new PhotoChannelContext())
            {
                List<Like> likes = context.Likes.Where(like => like.PhotoId == photo.Id).ToList();
                context.Likes.RemoveRange(likes);
                context.Photos.Remove(photo);
                context.SaveChanges();
            }
        }

        public void DeleteLike(Like like)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.Likes.FirstOrDefault(l => l.PhotoId == like.PhotoId && l.UserId == like.UserId);
                context.Likes.Remove(result ?? throw new InvalidOperationException());
                Photo photo = Get(p => p.Id == like.PhotoId);
                photo.LikesCount -= 1;
                context.SaveChanges();
            }
        }

        public void AddLike(Like like)
        {
            using (var context = new PhotoChannelContext())
            {
                context.Likes.Add(like);
                Photo photo = Get(p => p.Id == like.PhotoId);
                photo.LikesCount += 1;
                context.SaveChanges();
            }
        }
    }
}
