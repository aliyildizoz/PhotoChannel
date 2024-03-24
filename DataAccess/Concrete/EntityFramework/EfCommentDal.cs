using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCommentDal : EfEntityRepositoryBase<Comment, PhotoChannelContext>, ICommentDal
    {
        public PhotoChannelContext Context { get; private set; }
        public EfCommentDal(PhotoChannelContext context) : base(context)
        {
            Context = context;
        }
        public List<Photo> GetPhotosByUserComment(User user)
        {
            var photos = Context.Comments.Include(comment => comment.Photo).
                Include(comment => comment.Photo.User).
                Include(comment => comment.Photo.Channel).Where(like => like.UserId == user.Id).Where(comment => comment.UserId == user.Id).Select(comment => comment.Photo);
            return photos.ToList();
        }

        public List<User> GetUsersByPhotoComment(Photo photo)
        {
            var users = Context.Comments.Include(comment => comment.User)
                .Where(comment => comment.PhotoId == photo.Id).Select(comment => comment.User);
            return users.ToList();
        }

        public List<Comment> GetPhotoComments(Photo photo)
        {
            var comments = Context.Comments.Include(comment => comment.User)
                .Where(comment => comment.PhotoId == photo.Id);
            return comments.ToList();
        }
    }
}
