using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation.FluentValidation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class ChannelManager : IChannelService
    {
        private IChannelDal _channelDal;
        private IPhotoService _photoService;

        public ChannelManager(IChannelDal channelDal, IPhotoService photoService)
        {
            _channelDal = channelDal;
            _photoService = photoService;
        }

        public IDataResult<List<Channel>> GetList()
        {
            return new SuccessDataResult<List<Channel>>(_channelDal.GetList().ToList());
        }

        public IDataResult<Channel> GetById(int id)
        {
            return new SuccessDataResult<Channel>(_channelDal.Get(channel => channel.Id == id));
        }

        public IDataResult<List<Channel>> GetByName(string name)
        {
            return new SuccessDataResult<List<Channel>>(_channelDal.GetList(channel => channel.Name.Contains(name)).ToList());
        }
        public IDataResult<List<Photo>> GetPhotos(Channel channel)
        {
            return _photoService.GetPhotosByChannel(channel);
        }

        public IDataResult<List<User>> GetAdminList(Channel channel)
        {
            return new SuccessDataResult<List<User>>(_channelDal.GetAdminList(channel));
        }

        public IDataResult<List<User>> GetSubscribers(Channel channel)
        {
            return new SuccessDataResult<List<User>>(_channelDal.GetSubscriberList(channel));
        }
        [ValidationAspect(typeof(ChannelValidator), Priority = 1)]
        public IDataResult<Channel> Delete(Channel channel)
        {
            _channelDal.Delete(channel);
            return new SuccessDataResult<Channel>(Messages.ChannelDeleted, channel);
        }
        [ValidationAspect(typeof(ChannelValidator), Priority = 1)]
        public IDataResult<Channel> Add(Channel channel)
        {
            channel.CreatedDate = DateTime.Now;
            _channelDal.Add(channel);
            return new SuccessDataResult<Channel>(Messages.ChannelAdded, channel);
        }
        [ValidationAspect(typeof(ChannelValidator), Priority = 1)]
        public IDataResult<Channel> Update(Channel channel)
        {
            _channelDal.Update(channel);
            return new SuccessDataResult<Channel>(Messages.ChannelUpdated, channel);
        }
    }
}
