using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICommentService
    {
        IDataResult<List<Comment>> GetListByUserId(int userId);
        IDataResult<List<Comment>> GetListByPhotoId(int photoId);
        IDataResult<Comment> Delete(Comment comment);
        IDataResult<Comment> Add(Comment comment);
        IDataResult<Comment> Update(Comment comment);
    }
}
