using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class HomeManager:IHomeService
    {
        private IHomeDal _homeDal;
        public HomeManager(IHomeDal homeDal)
        {
            _homeDal = homeDal;
        }
        public IDataResult<List<Channel>> GetMostChannels()
        {
            return new SuccessDataResult<List<Channel>>(_homeDal.GetMostChannels());
        }

        public IDataResult<List<Photo>> GetMostComment()
        {
            return new SuccessDataResult<List<Photo>>(_homeDal.GetMostComment());
        }

        public IDataResult<List<Photo>> GetMostPhotos()
        {
            return new SuccessDataResult<List<Photo>>(_homeDal.GetMostPhotos());
        }

        public IDataResult<List<Photo>> GetFeed(int userId)
        {
            return new SuccessDataResult<List<Photo>>(_homeDal.GetFeed(userId));
        }
    }
}
