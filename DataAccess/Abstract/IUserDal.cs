using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        List<OperationClaim> GetClaims(User user);
        List<Channel> GetSubscriptionList(User user);
        List<Channel> GetChannels(User user);
        List<PhotoCardDto> GetLikedPhotos(User user);
        List<PhotoCardDto> GetSharedPhotos(User user);
        void AddOperationClaim(User user);
    }
}
