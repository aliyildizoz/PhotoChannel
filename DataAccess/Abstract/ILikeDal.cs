using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface ILikeDal : IEntityRepository<Like>
    {
        List<Photo> GetLikePhotos(User user);
        List<User> GetPhotoLikes(Photo photo);
    }
}
