using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Castle.Core.Internal;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    public class ChannelManager : IChannelService
    {
        private IChannelDal _channelDal;
        public ChannelManager(IChannelDal channelDal)
        {
            _channelDal = channelDal;
        }
        [CacheAspect]
        public IDataResult<List<Channel>> GetList()
        {
            return new SuccessDataResult<List<Channel>>(_channelDal.GetList().ToList());
        }
        [CacheAspect]
        public IDataResult<Channel> GetById(int id)
        {
            var channel = _channelDal.Get(c => c.Id == id);
            if (channel != null)
            {
                return new SuccessDataResult<Channel>(channel);
            }
            return new ErrorDataResult<Channel>(Messages.ChannelNotFound);
        }
        [CacheAspect]
        public IDataResult<User> GetOwner(int id)
        {
            var result = ChannelExists(id);
            if (!result.IsSuccessful)
            {
                return new ErrorDataResult<User>(result.Message);
            }
            return new SuccessDataResult<User>(_channelDal.Get(channel => channel.User, channel => channel.Id == id).User);
        }
        [CacheAspect]
        public IDataResult<List<Channel>> GetByName(string name)
        {
            return new SuccessDataResult<List<Channel>>(_channelDal.GetList(channel => channel.Name.Contains(name)).ToList());
        }
        [CacheAspect]
        public IDataResult<List<Photo>> GetPhotos(int id)
        {
            var result = ChannelExists(id);
            if (!result.IsSuccessful)
            {
                return new ErrorDataResult<List<Photo>>(result.Message);
            }
            return new SuccessDataResult<List<Photo>>(_channelDal.GetPhotos(new Channel { Id = id }));
        }

        public IDataResult<List<ChannelAdmin>> GetAdmins(int id)
        {
            var result = ChannelExists(id);
            if (!result.IsSuccessful)
            {
                return new ErrorDataResult<List<ChannelAdmin>>(result.Message);
            }
            return new SuccessDataResult<List<ChannelAdmin>>(_channelDal.GetAdminList(new Channel { Id = id }));
        }

        public IDataResult<List<Subscriber>> GetSubscribers(int id)
        {
            var result = ChannelExists(id);
            if (!result.IsSuccessful)
            {
                return new ErrorDataResult<List<Subscriber>>(result.Message);
            }
            return new SuccessDataResult<List<Subscriber>>(_channelDal.GetSubscribers(new Channel { Id = id }));
        }

        public IDataResult<List<Category>> GetCategories(int id)
        {
            var result = ChannelExists(id);
            if (!result.IsSuccessful)
            {
                return new ErrorDataResult<List<Category>>(result.Message);
            }
            return new SuccessDataResult<List<Category>>(_channelDal.GetCategories(new Channel { Id = id }));
        }

        [CacheRemoveAspect("IChannelService.Get")]
        public IResult Delete(int id)
        {
            _channelDal.Delete(new Channel { Id = id });
            return new SuccessResult(Messages.ChannelDeleted);
        }

        [ValidationAspect(typeof(ChannelValidator), Priority = 1)]
        [CacheRemoveAspect("IChannelService.Get")]
        public IDataResult<Channel> Add(Channel channel)
        {
            IResult result = BusinessRules.Run(CheckIfChannelNameExists(channel.Name));
            if (!result.IsSuccessful)
            {
                return new SuccessDataResult<Channel>(result.Message, channel);
            }
            _channelDal.Add(channel);
            return new SuccessDataResult<Channel>(Messages.ChannelAdded, channel);
        }

        public IResult CheckIfChannelNameExists(string channelName)
        {
            var result = _channelDal.GetList(channel => channel.Name == channelName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ChannelNameAlreadyExists);
            }
            return new SuccessResult();
        }

        [ValidationAspect(typeof(ChannelValidator), Priority = 1)]
        [CacheRemoveAspect("IChannelService.Get")]
        public IDataResult<Channel> Update(Channel channel, int channelId)
        {
            //Todo: direkt update işlemini dene
            _channelDal.Update(channel);
            return new SuccessDataResult<Channel>(Messages.ChannelUpdated, channel);
        }

        public IResult ChannelExists(int id)
        {
            var channel = _channelDal.Get(c => c.Id == id);
            if (channel != null)
            {
                return new SuccessResult();
            }
            return new ErrorResult(Messages.ChannelNotFound);
        }
        public IResult CheckIfChannelNameExistsWithUpdate(string name, int channelId)
        {
            Channel result = _channelDal.Get(channel => channel.Name == name);
            if (result != null)
            {
                return result.Id == channelId ? (IResult)new ErrorResult() : new SuccessResult(Messages.ChannelNameAlreadyExists);
            }
            return new ErrorResult();
        }
    }
}
