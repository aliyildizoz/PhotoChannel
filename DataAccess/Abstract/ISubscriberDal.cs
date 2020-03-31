using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface ISubscriberDal : IEntityRepository<Subscriber>
    {
        List<User> GetSubscribers(Channel channel);
        List<Channel> GetSubscriptions(User user);
    }
}
