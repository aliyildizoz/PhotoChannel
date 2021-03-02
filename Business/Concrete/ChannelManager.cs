using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    public class ChannelManager : IChannelService
    {
        private IChannelDal _channelDal;

        private Validation<ChannelValidator> _validation;
        public ChannelManager(IChannelDal channelDal)
        {
            _channelDal = channelDal;
        }

        public IDataResult<List<Channel>> GetList()
        {
            return new SuccessDataResult<List<Channel>>(_channelDal.GetList().ToList());
        }

        public IDataResult<Channel> GetById(int id)
        {
            var channel = _channelDal.Get(c => c.Id == id);
            if (channel != null)
            {
                return new SuccessDataResult<Channel>(channel);
            }
            return new ErrorDataResult<Channel>(Messages.ChannelNotFound);
        }

        public IDataResult<User> GetOwner(int id)
        {
            var result = Contains(new Channel { Id = id });
            if (!result)
            {
                return new ErrorDataResult<User>(Messages.ChannelNotFound);
            }
            return new SuccessDataResult<User>(_channelDal.GetOwner(new Channel { Id = id }));
        }
        
        public IDataResult<List<Channel>> GetByName(string name)
        {
            return new SuccessDataResult<List<Channel>>(_channelDal.GetList(channel => channel.Name.ToLower().Contains(name.ToLower())).ToList());
        }

        public IResult Delete(int id)
        {
            _channelDal.Delete(new Channel { Id = id });
            return new SuccessResult(Messages.ChannelDeleted);
        }


        public IDataResult<Channel> Add(Channel channel)
        {
            _validation = new Validation<ChannelValidator>();
            _validation.Validate(channel);
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

        public IDataResult<Channel> Update(Channel channel)
        {
            _validation = new Validation<ChannelValidator>();
            _validation.Validate(channel);
            _channelDal.Update(channel);
            return new SuccessDataResult<Channel>(Messages.ChannelUpdated, channel);
        }

        public IDataResult<List<Channel>> GetUserChannels(int userId)
        {
            return new SuccessDataResult<List<Channel>>(_channelDal.GetList(channel => channel.UserId == userId).ToList());
        }

        public IResult GetIsOwner(int channelId, int userId)
        {
            var result = GetOwner(channelId);
            return result.IsSuccessful ? result.Data.Id == userId ? (IResult)new SuccessResult() : new ErrorResult() : new ErrorResult();
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

        public bool Contains(IEntity entity)
        {
            return _channelDal.Contains(new Channel{ Id = entity.Id });
        }
    }
}
