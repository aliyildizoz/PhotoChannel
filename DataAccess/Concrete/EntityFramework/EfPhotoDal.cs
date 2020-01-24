using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;

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

    }
}
