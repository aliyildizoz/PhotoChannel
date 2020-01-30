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

        IResult Delete(Channel channel);
        IResult DeleteSubscribe(Subscriber subscriber);
        IResult AddSubscribe(Subscriber subscriber);
        IResult DeleteChannelAdmin(ChannelAdmin channelAdmin);
        IResult AddChannelAdmin(ChannelAdmin channelAdmin);
        IResult Add(Channel channel);
        IResult Update(Channel channel);
    }
}
