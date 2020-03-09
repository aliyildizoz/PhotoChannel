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
                Channel channel = Get(c => c.Id == subscriber.ChannelId);
                channel.SubscribersCount += 1;
                UserDetail userDetail = context.UserDetails.FirstOrDefault(u => u.UserId == subscriber.UserId);
                if (userDetail != null) userDetail.SubscriptionCount += 1;
                context.SaveChanges();
            }
        }

        public void AddChannelCategory(ChannelCategory channelCategory)
        {
            using (var context = new PhotoChannelContext())
            {
                context.ChannelCategories.Add(channelCategory);
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
        public void DeleteChannelCategory(ChannelCategory channelCategory)
        {
            using (var context = new PhotoChannelContext())
            {
                var category = context.ChannelCategories.FirstOrDefault(c =>
                    c.ChannelId == channelCategory.ChannelId && c.CategoryId == channelCategory.CategoryId);
                context.ChannelCategories.Remove(category ?? throw new InvalidOperationException());
                context.SaveChanges();
            }
        }
        public void DeleteSubscribe(Subscriber subscriber)
        {
            using (var context = new PhotoChannelContext())
            {
                var subs = context.Subscribers.FirstOrDefault(s =>
                    s.ChannelId == subscriber.ChannelId && s.UserId == subscriber.UserId);
                context.Subscribers.Remove(subs ?? throw new InvalidOperationException());
                Channel channel = Get(c => c.Id == subscriber.ChannelId);
                channel.SubscribersCount -= 1;
                UserDetail userDetail = context.UserDetails.FirstOrDefault(u => u.UserId == subscriber.UserId);
                if (userDetail != null) userDetail.SubscriptionCount -= 1;
                context.SaveChanges();
            }
        }


        public void DeleteChannelAdmin(ChannelAdmin channelAdmin)
        {
            using (var context = new PhotoChannelContext())
            {
                var admin = context.ChannelAdmins.FirstOrDefault(c =>
                    c.ChannelId == channelAdmin.ChannelId && c.UserId == channelAdmin.UserId);
                context.ChannelAdmins.Remove(admin ?? throw new InvalidOperationException());
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

        public void RelatedDelete(Channel channel)
        {
            using (var context = new PhotoChannelContext())
            {
                List<Subscriber> subscribers =
                    context.Subscribers.Where(subscriber => subscriber.ChannelId == channel.Id).ToList();
                List<ChannelAdmin> channelAdmins =
                    context.ChannelAdmins.Where(admin => admin.ChannelId == channel.Id).ToList();
                context.Subscribers.RemoveRange(subscribers);
                context.ChannelAdmins.RemoveRange(channelAdmins);
                context.Channels.Remove(channel);
                context.SaveChanges();
            }
        }
    }
}
