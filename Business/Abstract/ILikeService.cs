using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ILikeService
    {
        IDataResult<List<Photo>> GetLikePhotos(int userId);
        IDataResult<List<User>> GetPhotoLikes(int photoId);
        bool GetIsUserLike(int photoId, int userId);
        IDataResult<Like> Add(Like like);
        IResult Delete(Like like);
    }
}
