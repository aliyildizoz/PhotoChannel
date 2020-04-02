using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class ChannelAdminManager : IChannelAdminService
    {
        private IChannelAdminDal _channelAdminDal;

        public ChannelAdminManager(IChannelAdminDal channelAdminDal)
        {
            _channelAdminDal = channelAdminDal;
        }

        public IDataResult<List<User>> GetChannelAdmins(int channelId)
        {
            return new SuccessDataResult<List<User>>(_channelAdminDal.GetChannelAdmins(new Channel { Id = channelId }));
        }

        public IDataResult<List<Channel>> GetAdminChannels(int userId)
        {
            return new SuccessDataResult<List<Channel>>(_channelAdminDal.GetAdminChannels(new User { Id = userId }));
        }

        public IDataResult<ChannelAdmin> Add(ChannelAdmin channelAdmin)
        {
            _channelAdminDal.Add(channelAdmin);
            return new SuccessDataResult<ChannelAdmin>(channelAdmin);
        }

        public IResult Delete(ChannelAdmin channelAdmin)
        {
            //Todo:direkt olarak silme işleminide dene
            _channelAdminDal.Delete(_channelAdminDal.Get(channelAdmin1 => channelAdmin1.UserId == channelAdmin.UserId && channelAdmin1.ChannelId == channelAdmin.ChannelId));
            return new SuccessResult();
        }
    }
}
