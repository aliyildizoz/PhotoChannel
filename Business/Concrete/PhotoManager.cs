using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class PhotoManager : IPhotoService
    {
        private IPhotoDal _photoDal;

        public PhotoManager(IPhotoDal photoDal)
        {
            _photoDal = photoDal;
        }

        [CacheAspect]
        public IDataResult<List<User>> GetLikeUsersByPhoto(Photo photo)
        {
            return new SuccessDataResult<List<User>>(_photoDal.GetLikeUsersByPhoto(photo));
        }

        [CacheAspect]
        public IDataResult<List<Photo>> GetPhotosByUser(User user)
        {
            return new SuccessDataResult<List<Photo>>(_photoDal.GetList(photo => photo.UserId == user.Id).ToList());
        }

        [CacheAspect]
        public IDataResult<List<Photo>> GetPhotosByChannel(Channel channel)
        {
            return new SuccessDataResult<List<Photo>>(_photoDal.GetList(photo => photo.ChannelId == channel.Id).ToList());
        }

        [ValidationAspect(typeof(PhotoValidator), Priority = 1)]
        [CacheAspect]
        public IDataResult<Photo> GetById(int id)
        {
            return new SuccessDataResult<Photo>(_photoDal.Get(photo => photo.Id == id));
        }

        [CacheRemoveAspect("IPhotoService.Get")]
        public IResult Delete(Photo photo)
        {
            _photoDal.RelatedDelete(photo);
            return new SuccessResult();
        }
        [CacheRemoveAspect("IPhotoService.Get")]
        public IResult DeleteLike(Like like)
        {
            _photoDal.DeleteLike(like);
            return new SuccessResult();
        }
        [CacheRemoveAspect("IPhotoService.Get")]
        public IResult AddLike(Like like)
        {
            _photoDal.AddLike(like);
            return new SuccessResult();
        }

        [ValidationAspect(typeof(PhotoValidator), Priority = 1)]
        [CacheRemoveAspect("IPhotoService.Get")]
        public IResult Add(Photo photo)
        {
            _photoDal.Add(photo);
            return new SuccessResult();
        }

    }
}
