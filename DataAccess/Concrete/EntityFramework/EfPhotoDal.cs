    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfPhotoDal : EfEntityRepositoryBase<Photo, PhotoChannelContext>, IPhotoDal
    {
        public List<Photo> GetUserPhotos(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var photos = context.Photos.Include(photo => photo.User).Include(photo => photo.Channel)
                     .Where(photo => photo.UserId == user.Id);
                return photos.ToList();
            }
        }

        public List<Photo> GetChannelPhotos(Channel channel)
        {
            using (var context = new PhotoChannelContext())
            {
                var photos = context.Photos.Include(photo => photo.User).Include(photo => photo.Channel)
                    .Where(photo => photo.ChannelId == channel.Id);
                return photos.ToList();
            }
        }

        //public override void Delete(Photo entity)
        //{
        //    using (var context = new PhotoChannelContext())
        //    {
        //        var photos = context.
        //        base.Delete(entity);
        //    }

        //}
    }
}
