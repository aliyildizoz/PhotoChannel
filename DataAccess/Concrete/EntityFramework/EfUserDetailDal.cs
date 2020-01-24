using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDetailDal : EfEntityRepositoryBase<UserDetail, PhotoChannelContext>, IUserDetailDal
    {

    }
}
