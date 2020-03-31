using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, PhotoChannelContext>, IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.UserOperationClaims
                    .Where(userOperationClaim => userOperationClaim.UserId == user.Id).Join(context.OperationClaims,
                        userOperationClaim => userOperationClaim.OperationClaimId,
                        operationClaimId => operationClaimId.Id,
                        (userOperationClaim, operationClaim) => operationClaim);
                return result.ToList();
            }
        }
        public void AddOperationClaim(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                context.UserOperationClaims.Add(new UserOperationClaim
                {
                    UserId = user.Id,
                    OperationClaimId = 2
                });
                context.SaveChanges();
            }
        }
    }
}
