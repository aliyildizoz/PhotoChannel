using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class ChannelCategoryManager : IChannelCategoryService
    {
        private IChannelCategoryDal _channelCategoryDal;
        public ChannelCategoryManager(IChannelCategoryDal channelCategoryDal)
        {
            _channelCategoryDal = channelCategoryDal;
        }

        public IDataResult<List<Category>> GetChannelCategories(int channelId)
        {
            return new SuccessDataResult<List<Category>>(_channelCategoryDal.GetChannelCategories(new Channel { Id = channelId }));
        }

        public IDataResult<List<Channel>> GetCategoryChannels(int categoryId)
        {
            return new SuccessDataResult<List<Channel>>(_channelCategoryDal.GetCategoryChannels(new Category { Id = categoryId }));
        }

        public IDataResult<ChannelCategory> Add(ChannelCategory channelCategory)
        {
            _channelCategoryDal.Add(channelCategory);
            return new SuccessDataResult<ChannelCategory>(channelCategory);
        }

        public IResult AddRange(ChannelCategory[] channelCategories)
        {
            _channelCategoryDal.AddRange(channelCategories);
            return new SuccessResult();
        }

        public IResult Delete(ChannelCategory channelCategory)
        {
            _channelCategoryDal.Delete(_channelCategoryDal.Get(category => category.CategoryId == channelCategory.CategoryId && category.ChannelId == channelCategory.ChannelId));
            return new SuccessResult();
        }
    }
}
