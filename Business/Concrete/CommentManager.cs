using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CommentManager : ICommentService
    {
        private ICommentDal _commentDal;

        public CommentManager(ICommentDal commentDal)
        {
            _commentDal = commentDal;
        }

        public IDataResult<List<Comment>> GetListByUserId(int userId)
        {
            return new SuccessDataResult<List<Comment>>(_commentDal.GetList(comment => comment.UserId == userId).ToList());
        }

        public IDataResult<List<Comment>> GetListByPhotoId(int photoId)
        {
            return new SuccessDataResult<List<Comment>>(_commentDal.GetList(comment => comment.PhotoId == photoId).ToList());
        }

        public IDataResult<Comment> Delete(Comment comment)
        {
            _commentDal.Delete(comment);
            return new SuccessDataResult<Comment>(comment);
        }

        public IDataResult<Comment> Add(Comment comment)
        {
            _commentDal.Add(comment);
            return new SuccessDataResult<Comment>(comment);
        }

        public IDataResult<Comment> Update(Comment comment)
        {
            _commentDal.Update(comment);
            return new SuccessDataResult<Comment>(comment);
        }
    }
}
