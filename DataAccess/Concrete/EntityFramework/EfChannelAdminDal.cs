using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfChannelAdminDal : EfEntityRepositoryBase<ChannelAdmin, PhotoChannelContext>, IChannelAdminDal
    {
        public List<User> GetChannelAdmins(Channel channel)
        {
            using (var context = new PhotoChannelContext())
            {
                var admins = context.ChannelAdmins.Include(admin => admin.User).Where(admin => admin.ChannelId == channel.Id)
                     .Select(admin => admin.User);
                return admins.ToList();
            }
        }
        public List<Channel> GetAdminChannels(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var channels = context.ChannelAdmins.Include(admin => admin.Channel).Where(admin => admin.UserId == user.Id)
                    .Select(admin => admin.Channel);
                return channels.ToList();
            }
        }
    }
}
