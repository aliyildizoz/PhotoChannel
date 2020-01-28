using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Exception;
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
        [CacheAspect]
        [ExceptionAspect(typeof(IDataResult<List<Channel>>))]
        public IDataResult<List<Channel>> GetList()
        {
            //int a = 0;
            //int b = 5;
            //int c = b / a;
            return new SuccessDataResult<List<Channel>>(_channelDal.GetList().ToList());
        }
        [CacheAspect]
        public IDataResult<Channel> GetById(int id)
        {
            return new SuccessDataResult<Channel>(_channelDal.Get(channel => channel.Id == id));
        }
        [CacheAspect]
        public IDataResult<List<Channel>> GetByName(string name)
        {
            return new SuccessDataResult<List<Channel>>(_channelDal.GetList(channel => channel.Name.Contains(name)).ToList());
        }

        [CacheAspect]
        public IDataResult<List<Photo>> GetPhotos(Channel channel)
        {
            return _photoService.GetPhotosByChannel(channel);
        }

        [CacheAspect]
        public IDataResult<List<User>> GetAdminList(Channel channel)
        {
            return new SuccessDataResult<List<User>>(_channelDal.GetAdminList(channel));
        }

        [CacheAspect]
        public IDataResult<List<User>> GetSubscribers(Channel channel)
        {
            return new SuccessDataResult<List<User>>(_channelDal.GetSubscriberList(channel));
        }

        [CacheRemoveAspect("IChannelService.Get")]
        public IResult DeleteSubscribe(Subscriber subscriber)
        {
            _channelDal.DeleteSubscribe(subscriber);
            return new SuccessResult(Messages.SubscribeDeleted);
        }

        [CacheRemoveAspect("IChannelService.Get")]
        public IResult AddSubscribe(Subscriber subscriber)
        {
            _channelDal.AddSubscribe(subscriber);
            return new SuccessResult(Messages.SubscribeAdded);
        }

        [CacheRemoveAspect("IChannelService.Get")]
        public IResult DeleteChannelAdmin(ChannelAdmin channelAdmin)
        {
            _channelDal.DeleteChannelAdmin(channelAdmin);
            return new SuccessResult(Messages.ChannelAdminDeleted);
        }

        [CacheRemoveAspect("IChannelService.Get")]
        public IResult AddChannelAdmin(ChannelAdmin channelAdmin)
        {
            _channelDal.AddChannelAdmin(channelAdmin);
            return new SuccessResult(Messages.ChannelAdminAdded);
        }

        [CacheRemoveAspect("IChannelService.Get")]
        public IResult Delete(Channel channel)
        {
            _channelDal.RelatedDelete(channel);
            return new SuccessResult(Messages.ChannelDeleted);
        }

        [ValidationAspect(typeof(ChannelValidator), Priority = 1)]
        [CacheRemoveAspect("IChannelService.Get")]
        public IDataResult<Channel> Add(Channel channel)
        {
            channel.CreatedDate = DateTime.Now;
            _channelDal.Add(channel);
            return new SuccessDataResult<Channel>(Messages.ChannelAdded, channel);
        }

        [ValidationAspect(typeof(ChannelValidator), Priority = 1)]
        [CacheRemoveAspect("IChannelService.Get")]
        public IDataResult<Channel> Update(Channel channel)
        {
            _channelDal.Update(channel);
            return new SuccessDataResult<Channel>(Messages.ChannelUpdated, channel);
        }
    }
}
