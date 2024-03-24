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
        public PhotoChannelContext Context { get; private set; }
        public EfUserDal(PhotoChannelContext context) : base(context)
        {
            Context = context;
        }
        public List<OperationClaim> GetClaims(User user)
        {
            var result = Context.UserOperationClaims
                .Where(userOperationClaim => userOperationClaim.UserId == user.Id).Join(Context.OperationClaims,
                    userOperationClaim => userOperationClaim.OperationClaimId,
                    operationClaimId => operationClaimId.Id,
                    (userOperationClaim, operationClaim) => operationClaim);
            return result.ToList();
        }
        public void AddOperationClaim(User user)
        {
            Context.UserOperationClaims.Add(new UserOperationClaim
            {
                UserId = user.Id,
                OperationClaimId = 2
            });
            Context.SaveChanges();
        }
    }
}
