using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Abstract
{
    public interface IChannelDal : IEntityRepository<Channel>
    {
        User GetOwner(Channel channel);
    }
}
