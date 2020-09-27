using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICommentService
    {
        IDataResult<List<Photo>> GetPhotosByUserComment(int userId);
        IDataResult<List<User>> GetUsersByPhotoComment(int photoId);
        IDataResult<List<Comment>> GetPhotoComments(int photoId);
        IDataResult<Comment> GetById(int commentId);
        IResult Delete(Comment comment);
        IResult Add(Comment comment);
        IResult Update(Comment comment);
    }
}
