using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Internal;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfChannelDal : EfEntityRepositoryBase<Channel, PhotoChannelContext>, IChannelDal
    {
        public User GetOwner(Channel channel)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.Channels.Include(c => c.User).SingleOrDefault(c => c.Id == channel.Id);
                return result?.User;
            }
        }
    }
}
