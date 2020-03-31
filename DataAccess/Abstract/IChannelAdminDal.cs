using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IChannelAdminDal : IEntityRepository<ChannelAdmin>
    {
        List<User> GetChannelAdmins(Channel channel);
        List<Channel> GetAdminChannels(User user);
    }
}
