using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface ICountDal
    {
        int GetPhotoLikeCount(Photo photo);
        int GetPhotoCommentCount(Photo photo);

        int GetSubscriberCount(Channel channel);
        int GetSubscriptionsCount(User user);


    }
}
