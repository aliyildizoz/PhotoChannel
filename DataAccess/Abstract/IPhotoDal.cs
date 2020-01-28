using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using Core.Entities.Concrete;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IPhotoDal : IEntityRepository<Photo>
    {
        List<User> GetLikeUsersByPhoto(Photo photo);
        void RelatedDelete(Photo photo);
        void DeleteLike(Like like);
        void AddLike(Like like);
    }
}
