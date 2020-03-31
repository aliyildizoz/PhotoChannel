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
        List<Photo> GetUserPhotos(User user);
        List<Photo> GetChannelPhotos(Channel channel);
    }
}
