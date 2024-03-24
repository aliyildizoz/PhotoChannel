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
    public class EfSubscriberDal : EfEntityRepositoryBase<Subscriber, PhotoChannelContext>, ISubscriberDal
    {
        public PhotoChannelContext Context { get; private set; }
        public EfSubscriberDal(PhotoChannelContext context) : base(context)
        {
            Context = context;
        }
        public List<User> GetSubscribers(Channel channel)
        {
            var photos = Context.Subscribers.Include(subscriber => subscriber.User)
                .Where(subscriber => subscriber.ChannelId == channel.Id).Select(subscriber => subscriber.User);
            return photos.ToList();
        }
        public List<Channel> GetSubscriptions(User user)
        {
            var channels = Context.Subscribers.Include(subscriber => subscriber.Channel)
                .Where(subscriber => subscriber.UserId == user.Id).Select(subscriber => subscriber.Channel);
            return channels.ToList();
        }

        public override void Add(Subscriber entity)
        {
            var contains = Context.Subscribers.Contains(entity);
            if (!contains) base.Add(entity);
        }
        public override void Delete(Subscriber entity)
        {
            var contains = Context.Subscribers.Contains(entity);
            if (contains) base.Delete(entity);
        }
    }
}
