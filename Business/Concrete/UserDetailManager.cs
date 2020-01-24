using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
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

        public IDataResult<UserDetail> GetByUserId(int userId)
        {
            return new SuccessDataResult<UserDetail>(_userDetailDal.Get(userDetail => userDetail.UserId == userId));
        }
    }
}
