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

        public IDataResult<List<Photo>> GetPhotosByUserComment(int userId)
        {
            return new SuccessDataResult<List<Photo>>(_commentDal.GetPhotosByUserComment(new User { Id = userId }));
        }

        public IDataResult<List<User>> GetUsersByPhotoComment(int photoId)
        {
            return new SuccessDataResult<List<User>>(_commentDal.GetUsersByPhotoComment(new Photo { Id = photoId }));
        }

        public IDataResult<List<Comment>> GetPhotoComments(int photoId)
        {
            return new SuccessDataResult<List<Comment>>(_commentDal.GetPhotoComments(new Photo { Id = photoId }).ToList());
        }

        [ValidationAspect(typeof(CommentValidator), Priority = 1)]
        [CacheRemoveAspect("ICommentService.Get")]
        public IResult Delete(Comment comment)
        {
            _commentDal.Delete(comment);
            return new SuccessDataResult<Comment>(comment);
        }

        [ValidationAspect(typeof(CommentValidator), Priority = 1)]
        [CacheRemoveAspect("ICommentService.Get")]
        public IResult Add(Comment comment)
        {
            _commentDal.Add(comment);
            return new SuccessDataResult<Comment>(comment);
        }

        [ValidationAspect(typeof(CommentValidator), Priority = 1)]
        [CacheRemoveAspect("ICommentService.Get")]
        public IResult Update(Comment comment)
        {
            _commentDal.Update(comment);
            return new SuccessResult();
        }
    }
}
