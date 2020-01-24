using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IPhotoService
    {
        IDataResult<List<User>> GetLikeUsersByPhoto(Photo photo);
        IDataResult<List<Photo>> GetPhotosByUser(User user);
        IDataResult<List<Photo>> GetPhotosByChannel(Channel channel);
        IDataResult<Photo> Delete(Photo photo);
        IDataResult<Photo> Add(Photo photo);
        IDataResult<Photo> GetById(int id);
    }
}
