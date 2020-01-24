using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using Core.Entities.Concrete;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IChannelDal : IEntityRepository<Channel>
    {
        List<User> GetAdminList(Channel channel);
        List<User> GetSubscriberList(Channel channel);
        void AddSubscribe(Subscriber subscriber);
        void AddChannelAdmin(ChannelAdmin channelAdmin);
        void DeleteSubscribe(Subscriber subscriber);
        void DeleteChannelAdmin(ChannelAdmin channelAdmin);
        List<Channel> GetListByCategory(Category category);
    }
}
