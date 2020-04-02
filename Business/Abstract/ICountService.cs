using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICountService
    {
        IDataResult<int> GetPhotoLikeCount(int photoId);
        IDataResult<int> GetPhotoCommentCount(int photoId);

        IDataResult<int> GetSubscriberCount(int channelId);
        IDataResult<int> GetSubscriptionsCount(int userId);
    }
}
