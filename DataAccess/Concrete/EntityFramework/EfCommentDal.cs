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
        public List<Photo> GetPhotosByUserComment(User user)
        {
            using (var context = new PhotoChannelContext())
            {
                var photos = context.Comments.Include(comment => comment.Photo)
                    .Where(comment => comment.UserId == user.Id).Select(comment => comment.Photo);
                return photos.ToList();
            }
        }

        public List<User> GetUsersByPhotoComment(Photo photo)
        {
            using (var context = new PhotoChannelContext())
            {
                var users = context.Comments.Include(comment => comment.User)
                    .Where(comment => comment.PhotoId == photo.Id).Select(comment => comment.User);
                return users.ToList();
            }
        }

        public List<Comment> GetPhotoComments(Photo photo)
        {
            using (var context = new PhotoChannelContext())
            {
                var comments = context.Comments.Include(comment => comment.User)
                    .Where(comment => comment.PhotoId == photo.Id);
                return comments.ToList();
            }
        }
    }
}
