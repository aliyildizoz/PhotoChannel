using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IHomeService
    {
        IDataResult<List<Channel>> GetMostChannels();
        IDataResult<List<Photo>> GetMostComment();
        IDataResult<List<Photo>> GetMostPhotos();
        IDataResult<List<Photo>> GetFeed(int userId);
    }
}
