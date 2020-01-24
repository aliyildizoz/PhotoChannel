using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
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

        //UserManager a taşınmalı**
        public IDataResult<List<User>> GetLikeUsersByPhoto(Photo photo)
        {
            return new SuccessDataResult<List<User>>(_photoDal.GetLikeUsersByPhoto(photo));
        }

        public IDataResult<List<Photo>> GetPhotosByUser(User user)
        {
            return new SuccessDataResult<List<Photo>>(_photoDal.GetList(photo => photo.UserId == user.Id).ToList());
        }

        public IDataResult<List<Photo>> GetPhotosByChannel(Channel channel)
        {
            return new SuccessDataResult<List<Photo>>(_photoDal.GetList(photo => photo.ChannelId == channel.Id).ToList());
        }

        public IDataResult<Photo> Delete(Photo photo)
        {
            _photoDal.Delete(photo);
            return new SuccessDataResult<Photo>(photo);
        }

        public IDataResult<Photo> Add(Photo photo)
        {
            _photoDal.Add(photo);
            return new SuccessDataResult<Photo>(photo);
        }

        public IDataResult<Photo> GetById(int id)
        {
            return new SuccessDataResult<Photo>(_photoDal.Get(photo => photo.Id == id));
        }
    }
}
