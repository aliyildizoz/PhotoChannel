using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ISubscriberService
    {
        IDataResult<List<User>> GetSubscribers(int channelId);
        IDataResult<List<Channel>> GetSubscriptions(int userId);
        IDataResult<Subscriber> Add(Subscriber subscriber);
        IResult Delete(Subscriber subscriber);
        bool GetIsUserSubs(int channelId, int userId);

    }
}
