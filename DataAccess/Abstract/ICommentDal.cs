using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface ICommentDal : IEntityRepository<Comment>
    {
        List<Photo> GetPhotosByUserComment(User user);
        List<User> GetUsersByPhotoComment(Photo photo);
        List<Comment> GetPhotoComments(Photo photo);
    }
}
