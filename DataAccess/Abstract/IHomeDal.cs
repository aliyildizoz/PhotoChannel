using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IHomeDal
    {
        List<Channel> GetMostChannels();
        List<Photo> GetMostComment();
        List<Photo> GetMostPhotos();
        List<Photo> GetFeed(int userId);
    }
}
