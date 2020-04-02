using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CountManager : ICountService
    {
        private ICountDal _countDal;

        public CountManager(ICountDal countDal)
        {
            _countDal = countDal;
        }

        public IDataResult<int> GetPhotoLikeCount(int photoId)
        {
            return new DataResultBase<int>(true, _countDal.GetPhotoLikeCount(new Photo { Id = photoId }));
        }

        public IDataResult<int> GetPhotoCommentCount(int photoId)
        {
            return new DataResultBase<int>(true, _countDal.GetPhotoCommentCount(new Photo { Id = photoId }));
        }

        public IDataResult<int> GetSubscriberCount(int channelId)
        {
            return new DataResultBase<int>(true, _countDal.GetSubscriberCount(new Channel { Id = channelId }));
        }

        public IDataResult<int> GetSubscriptionsCount(int userId)
        {
            return new DataResultBase<int>(true, _countDal.GetSubscriptionsCount(new User { Id = userId }));
        }
    }
}
