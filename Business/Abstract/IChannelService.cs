using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IChannelService
    {
        IDataResult<List<Channel>> GetList();
        IDataResult<Channel> GetById(int id);
        IDataResult<List<Channel>> GetByName(string name);
        IDataResult<List<Photo>> GetPhotos(Channel channel);

        IDataResult<List<User>> GetAdminList(Channel channel);
        IDataResult<List<User>> GetSubscribers(Channel channel);

        IDataResult<Channel> Delete(Channel channel);
        IDataResult<Channel> Add(Channel channel);
        IDataResult<Channel> Update(Channel channel);
    }
}
