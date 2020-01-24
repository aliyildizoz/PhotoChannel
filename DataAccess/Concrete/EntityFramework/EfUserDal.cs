using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, PhotoChannelContext>, IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.UserOperationClaim
                    .Where(userOperationClaim => userOperationClaim.UserId == user.Id).Join(context.OperationClaims,
                        userOperationClaim => userOperationClaim.OperationClaimId,
                        operationClaimId => operationClaimId.Id,
                        (userOperationClaim, operationClaim) => operationClaim);
                return result.ToList();
            }
        }

        public List<Channel> GetSubscriptionList(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.Subscribers.Where(subscriber => subscriber.UserId == user.Id).Join(
                    context.Channels, subscriber => subscriber.ChannelId, channel => channel.Id,
                    (subscriber, channel) => channel);
                return result.ToList();
            }
        }
    }
}
