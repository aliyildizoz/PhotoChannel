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

namespace DataAccess.Concrete.EntityFramework
{
    public class EfChannelDal : EfEntityRepositoryBase<Channel, PhotoChannelContext>, IChannelDal
    {
        public List<User> GetAdminList(Channel channel)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.ChannelAdmins.Where(channelAdmin => channelAdmin.ChannelId == channel.Id).Join(
                    context.Users, channelAdmin => channelAdmin.UserId, user => user.Id,
                    (channelAdmin, user) => user);
                return result.ToList();
            }
        }

        public List<User> GetSubscriberList(Channel channel)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.Subscribers.Where(subscriber => subscriber.ChannelId == channel.Id).Join(
                    context.Users, channelAdmin => channelAdmin.UserId, user => user.Id,
                    (channelAdmin, user) => user);
                return result.ToList();
            }
        }

        public void AddSubscribe(Subscriber subscriber)
        {
            using (var context = new PhotoChannelContext())
            {
                context.Subscribers.Add(subscriber);
                context.SaveChanges();
            }
        }

        public void AddChannelAdmin(ChannelAdmin channelAdmin)
        {
            using (var context = new PhotoChannelContext())
            {
                context.ChannelAdmins.Add(channelAdmin);
                context.SaveChanges();
            }
        }

        public void DeleteSubscribe(Subscriber subscriber)
        {
            using (var context = new PhotoChannelContext())
            {
                context.Subscribers.Remove(subscriber);
                context.SaveChanges();
            }
        }

        public void DeleteChannelAdmin(ChannelAdmin channelAdmin)
        {
            using (var context = new PhotoChannelContext())
            {
                context.ChannelAdmins.Remove(channelAdmin);
                context.SaveChanges();
            }
        }

        public List<Channel> GetListByCategory(Category category)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.ChannelCategories.Where(channelCategory => channelCategory.CategoryId == category.Id).Join(
                    context.Channels, channelCategory => channelCategory.ChannelId, channel => channel.Id,
                    (channelCategory, channel) => channel);
                return result.ToList();
            }
        }

        public void RelatedDeleteChannel(Channel channel)
        {
            using (var context = new PhotoChannelContext())
            {
                
                DeleteChannelAdmin(new ChannelAdmin());
            }
        }
    }
}
