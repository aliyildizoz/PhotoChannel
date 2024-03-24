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
        public PhotoChannelContext Context { get; private set; }
        public EfHomeDal(PhotoChannelContext context)
        {
            Context = context;
        }
        public List<Channel> GetMostChannels()
        {
            Dictionary<Channel, int> dictionary = new Dictionary<Channel, int>();
            Context.Channels.ToList().ForEach(channel =>
            {
                dictionary.Add(channel,
                    Context.Subscribers.Count(subscriber => subscriber.ChannelId == channel.Id));
            });
            return dictionary.OrderByDescending(pair => pair.Value).Take(5).Select(pair => pair.Key).ToList();
        }

        public List<Photo> GetMostComment()
        {
            Dictionary<Photo, int> dictionary = new Dictionary<Photo, int>();
            Context.Photos.Include(photo => photo.Channel).Include(photo => photo.User).ToList().ForEach(photo =>
            {
                dictionary.Add(photo,
                    Context.Comments.Count(comment => comment.PhotoId == photo.Id));
            });
            return dictionary.OrderByDescending(pair => pair.Value).Take(10).Select(pair => pair.Key).ToList();
        }

        public List<Photo> GetMostPhotos()
        {
            Dictionary<Photo, int> dictionary = new Dictionary<Photo, int>();
            Context.Photos.Include(photo => photo.Channel).Include(photo => photo.User).ToList().ForEach(photo =>
            {
                dictionary.Add(photo,
                    Context.Likes.Count(like => like.PhotoId == photo.Id));
            });
            return dictionary.OrderByDescending(pair => pair.Value).Take(10).Select(pair => pair.Key).ToList();
        }

        public List<Photo> GetFeed(int userId)
        {
            List<Photo> photos = new List<Photo>();
            Context.Subscribers.Where(subscriber => subscriber.UserId == userId).ToList().ForEach(subscriber =>
            {
                photos.AddRange(Context.Photos.Include(photo => photo.Channel).Include(photo => photo.User).Where(photo => photo.ChannelId == subscriber.ChannelId).ToList());
            });
            return photos.OrderByDescending(photo => photo.ShareDate).ToList();
        }
    }
}
