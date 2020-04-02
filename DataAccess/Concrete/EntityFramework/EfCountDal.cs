using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCountDal : ICountDal
    {
        public int GetPhotoLikeCount(Photo photo)
        {
            using (var context = new PhotoChannelContext())
            {
                return context.Likes.Count(like => like.PhotoId == photo.Id);
            }
        }

        public int GetPhotoCommentCount(Photo photo)
        {
            using (var context = new PhotoChannelContext())
            {
                return context.Comments.Count(comment => comment.PhotoId == photo.Id);
            }
        }

        public int GetSubscriberCount(Channel channel)
        {
            using (var context = new PhotoChannelContext())
            {
                return context.Subscribers.Count(subscriber => subscriber.ChannelId == channel.Id);
            }
        }

        public int GetSubscriptionsCount(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                return context.Subscribers.Count(subscriber => subscriber.UserId == user.Id);
            }
        }
    }
}
