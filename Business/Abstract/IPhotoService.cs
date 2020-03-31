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
        IDataResult<List<Photo>> GetChannelPhotos(int channelId);
        IDataResult<List<Photo>> GetUserPhotos(int userId);
       
        IDataResult<Photo> GetById(int id);
        IDataResult<Photo> Add(Photo photo);
        IResult Delete(Photo photo);
        IResult Exists(int id);
    }
}
