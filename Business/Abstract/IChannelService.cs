using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IChannelService: ICommonService
    {
        IDataResult<List<Channel>> GetList();
        IDataResult<Channel> GetById(int id);
        IDataResult<List<Channel>> GetByName(string name);
        IDataResult<User> GetOwner(int id);
        IResult Delete(int id);
        IDataResult<Channel> Add(Channel channel);
        IDataResult<Channel> Update(Channel channel);
        IDataResult<List<Channel>> GetUserChannels(int userId);
        IResult GetIsOwner(int channelId, int userId);
        IResult CheckIfChannelNameExistsWithUpdate(string name, int id);
        IResult CheckIfChannelNameExists(string channelName);
    }
}
