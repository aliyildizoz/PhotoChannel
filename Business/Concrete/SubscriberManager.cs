using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class SubscriberManager : ISubscriberService
    {
        private ISubscriberDal _subscriberDal;

        public SubscriberManager(ISubscriberDal subscriberDal)
        {
            _subscriberDal = subscriberDal;
        }

        public IDataResult<List<User>> GetSubscribers(int channelId)
        {
            return new SuccessDataResult<List<User>>(_subscriberDal.GetSubscribers(new Channel { Id = channelId }));
        }

        public IDataResult<List<Channel>> GetSubscriptions(int userId)
        {
            return new SuccessDataResult<List<Channel>>(_subscriberDal.GetSubscriptions(new User { Id = userId }));
        }

        public IDataResult<Subscriber> Add(Subscriber subscriber)
        {
            _subscriberDal.Add(subscriber);
            return new SuccessDataResult<Subscriber>(subscriber);
        }

        public IResult Delete(Subscriber subscriber)
        {
            _subscriberDal.Delete(_subscriberDal.Get(subscriber1 => subscriber1.UserId == subscriber.UserId && subscriber1.ChannelId == subscriber1.ChannelId));
            return new SuccessResult();
        }
    }
}
