using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Core.Utilities.Results;

namespace Business.Abstract
{
    public interface IUserDetailService
    {
        IDataResult<UserDetail> GetByUserId(int userId);
        IResult Add(UserDetail userDetail);
    }
}
