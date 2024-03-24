using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCountDal : ICountDal
    {
        public PhotoChannelContext Context { get; private set; }
        public EfCountDal(PhotoChannelContext context)
        {
            Context = context;
        }
        public int GetPhotoLikeCount(Photo photo)
        {
                return Context.Likes.Count(like => like.PhotoId == photo.Id);
        }

        public int GetPhotoCommentCount(Photo photo)
        {
                return Context.Comments.Count(comment => comment.PhotoId == photo.Id);
        }

        public int GetSubscriberCount(Channel channel)
        {
                return Context.Subscribers.Count(subscriber => subscriber.ChannelId == channel.Id);
        }

        public int GetSubscriptionsCount(User user)
        {
                return Context.Subscribers.Count(subscriber => subscriber.UserId == user.Id);
        }
    }
}
