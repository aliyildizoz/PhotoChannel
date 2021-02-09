using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Abstract;
using DataAccess.Dal.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfHomeDal : IHomeDal
    {
        public List<Channel> GetMostChannels()
        {
            using (var context = new PhotoChannelContext())
            {
                Dictionary<Channel, int> dictionary = new Dictionary<Channel, int>();
                context.Channels.ToList().ForEach(channel =>
                {
                    dictionary.Add(channel,
                        context.Subscribers.Count(subscriber => subscriber.ChannelId == channel.Id));
                });
                return dictionary.OrderByDescending(pair => pair.Value).Take(5).Select(pair => pair.Key).ToList();
            }
        }

        public List<Photo> GetMostComment()
        {
            using (var context = new PhotoChannelContext())
            {
                Dictionary<Photo, int> dictionary = new Dictionary<Photo, int>();
                context.Photos.Include(photo => photo.Channel).Include(photo => photo.User).ToList().ForEach(photo =>
                {
                    dictionary.Add(photo,
                        context.Comments.Count(comment => comment.PhotoId== photo.Id));
                });
                return dictionary.OrderByDescending(pair => pair.Value).Take(10).Select(pair => pair.Key).ToList();
            }
        }

        public List<Photo> GetMostPhotos()
        {
            using (var context = new PhotoChannelContext())
            {
                Dictionary<Photo, int> dictionary = new Dictionary<Photo, int>();
                context.Photos.Include(photo => photo.Channel).Include(photo => photo.User).ToList().ForEach(photo =>
                {
                    dictionary.Add(photo,
                        context.Likes.Count(like => like.PhotoId == photo.Id));
                });
                return dictionary.OrderByDescending(pair => pair.Value).Take(10).Select(pair => pair.Key).ToList();
            }
        }

        public List<Photo> GetFeed(int userId)
        {
            using (var context = new PhotoChannelContext())
            {
                List<Photo> photos = new List<Photo>();
                context.Subscribers.Where(subscriber => subscriber.UserId==userId).ToList().ForEach(subscriber =>
                {
                    photos.AddRange(context.Photos.Include(photo => photo.Channel).Include(photo => photo.User).Where(photo => photo.ChannelId== subscriber.ChannelId).ToList());
                });
                return photos.OrderByDescending(photo => photo.ShareDate).ToList();
            }
        }
    }
}
