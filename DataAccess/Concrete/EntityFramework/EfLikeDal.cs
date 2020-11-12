using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfLikeDal : EfEntityRepositoryBase<Like, PhotoChannelContext>, ILikeDal
    {

        public List<Photo> GetLikePhotos(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var photos = context.Likes.Include(like => like.Photo).Include(like => like.Photo.User).Include(like => like.Photo.Channel).Where(like => like.UserId == user.Id)
                    .Select(like => like.Photo);
                return photos.ToList();
            }
        }

        public List<User> GetPhotoLikes(Photo photo)
        {
            using (var context = new PhotoChannelContext())
            {
                var users = context.Likes.Include(like => like.User).Where(like => like.PhotoId == photo.Id)
                    .Select(like => like.User);
                return users.ToList();
            }
        }

        public override void Delete(Like entity)
        {
            using (var context = new PhotoChannelContext())
            {
                var contains = context.Likes.Contains(entity);
                if (contains) base.Delete(entity);
            }
        }

        public override void Add(Like entity)
        {

            using (var context = new PhotoChannelContext())
            {
                var contains = context.Likes.Contains(entity);
                if (!contains) base.Add(entity);
            }
        }
    }
}
