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
        IResult Delete(Comment comment);
        IResult Add(Comment comment);
        IResult Update(Comment comment);
    }
}
