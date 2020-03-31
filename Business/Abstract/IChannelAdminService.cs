using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IChannelAdminsService
    {
        IDataResult<List<User>> GetChannelAdmins(int channelId);
        IDataResult<List<Channel>> GetAdminChannels(int userId);
        IDataResult<ChannelAdmin> Add(ChannelAdmin channelAdmin);
        IResult Delete(ChannelAdmin channelAdmin);
    }
}
