using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
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


        public IDataResult<List<Photo>> GetChannelPhotos(int channelId)
        {
            return new SuccessDataResult<List<Photo>>(_photoDal.GetChannelPhotos(new Channel { Id = channelId }));
        }

        public IDataResult<List<Photo>> GetUserPhotos(int userId)
        {
            return new SuccessDataResult<List<Photo>>(_photoDal.GetUserPhotos(new User { Id = userId }));
        }

        public IDataResult<Photo> GetById(int id)
        {
            var photo = _photoDal.Get(p => p.Id == id);
            if (photo != null)
            {
                return new SuccessDataResult<Photo>(photo);
            }
            return new ErrorDataResult<Photo>(Messages.PhotoNotFound);
        }

        public IResult Delete(Photo photo)
        {
            _photoDal.Delete(photo);
            return new SuccessResult();
        }

        public bool Contains(Photo photo)
        {
            return _photoDal.Contains(photo);
        }

        public IDataResult<Photo> Add(Photo photo)
        {
            Validation<PhotoValidator> validation = new Validation<PhotoValidator>();
            validation.Validate(photo);
            _photoDal.Add(photo);
            return new SuccessDataResult<Photo>(photo);
        }
    }
}
