using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
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

        [CacheAspect]
        public IDataResult<List<Comment>> GetListByUserId(int userId)
        {
            return new SuccessDataResult<List<Comment>>(_commentDal.GetList(comment => comment.UserId == userId).ToList());
        }

        [CacheAspect]
        public IDataResult<List<Comment>> GetListByPhotoId(int photoId)
        {
            return new SuccessDataResult<List<Comment>>(_commentDal.GetList(comment => comment.PhotoId == photoId).ToList());
        }

        [ValidationAspect(typeof(CommentValidator), Priority = 1)]
        [CacheRemoveAspect("ICommentService.Get")]
        public IDataResult<Comment> Delete(Comment comment)
        {
            _commentDal.Delete(comment);
            return new SuccessDataResult<Comment>(comment);
        }

        [ValidationAspect(typeof(CommentValidator), Priority = 1)]
        [CacheRemoveAspect("ICommentService.Get")]
        public IDataResult<Comment> Add(Comment comment)
        {
            _commentDal.Add(comment);
            return new SuccessDataResult<Comment>(comment);
        }

        [ValidationAspect(typeof(CommentValidator), Priority = 1)]
        [CacheRemoveAspect("ICommentService.Get")]
        public IDataResult<Comment> Update(Comment comment)
        {
            _commentDal.Update(comment);
            return new SuccessDataResult<Comment>(comment);
        }
    }
}
