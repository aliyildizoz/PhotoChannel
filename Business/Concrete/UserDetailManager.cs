using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;

namespace Business.Concrete
{
    public class UserDetailManager : IUserDetailService
    {
        private IUserDetailDal _userDetailDal;

        public UserDetailManager(IUserDetailDal userDetailDal)
        {
            _userDetailDal = userDetailDal;
        }
        [CacheAspect]
        public IDataResult<UserDetail> GetByUserId(int userId)
        {
            UserDetail userDetail1 = _userDetailDal.Get(userDetail => userDetail.UserId == userId);
            return new SuccessDataResult<UserDetail>(_userDetailDal.Get(userDetail => userDetail.UserId == userId));
        }

        public IResult Add(UserDetail userDetail)
        {
            _userDetailDal.Add(userDetail);
            return new SuccessResult();
        }
    }
}
