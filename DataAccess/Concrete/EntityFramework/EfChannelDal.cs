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
    public class EfChannelDal : EfEntityRepositoryBase<Channel, PhotoChannelContext>, IChannelDal
    {
        public PhotoChannelContext Context { get; private set; }
        public EfChannelDal(PhotoChannelContext context) : base(context)
        {
            Context = context;
        }

        public User GetOwner(Channel channel)
        {
            var result = Context.Channels.Include(c => c.User).SingleOrDefault(c => c.Id == channel.Id);
            return result?.User;
        }
    }
}
