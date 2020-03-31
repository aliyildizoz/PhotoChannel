using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IChannelService
    {
        IDataResult<List<Channel>> GetList();
        IDataResult<Channel> GetById(int id);
        IDataResult<List<Channel>> GetByName(string name);

        IDataResult<User> GetOwner(int id);
        IDataResult<List<Photo>> GetPhotos(int id);
        IDataResult<List<ChannelAdmin>> GetAdmins(int id);
        IDataResult<List<Subscriber>> GetSubscribers(int id);
        IDataResult<List<Category>> GetCategories(int id);

        IResult Delete(int id);
        IDataResult<Channel> Add(Channel channel);
        IDataResult<Channel> Update(Channel channel, int id);
        IResult ChannelExists(int id);
        IResult CheckIfChannelNameExistsWithUpdate(string name, int id);
        IResult CheckIfChannelNameExists(string channelName);
    }
}
