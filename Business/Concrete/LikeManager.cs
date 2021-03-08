using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrete
{
    public class LikeManager : ILikeService
    {
        private ILikeDal _likeDal;

        public LikeManager(ILikeDal likeDal)
        {
            _likeDal = likeDal;
        }

        public IDataResult<List<Photo>> GetLikePhotos(int userId)
        {
            return new SuccessDataResult<List<Photo>>(_likeDal.GetLikePhotos(new User { Id = userId }));
        }

        public IDataResult<List<User>> GetPhotoLikes(int photoId)
        {
            return new SuccessDataResult<List<User>>(_likeDal.GetPhotoLikes(new Photo { Id = photoId }));
        }

        public bool GetIsUserLike(int photoId, int userId)
        {
            var result = _likeDal.Get(like => like.UserId == userId && like.PhotoId == photoId);
            return result != null;
        }

        public IDataResult<Like> Add(Like like)
        {
            var like2 = _likeDal.Get(like1 => like1.UserId == id.UserId && like1.PhotoId == id.PhotoId)
            if (like2 == null)
            {
                _likeDal.Add(like);
                like2 = like;

            }
            return new SuccessDataResult<Like>(like2);
        }
        public IResult Delete(Like id)
        {
            var like = _likeDal.Get(like1 => like1.UserId == id.UserId && like1.PhotoId == id.PhotoId)
            if(like != null)
            {
                _likeDal.Delete(like);
            }
            return new SuccessResult();
        }
    }
}
