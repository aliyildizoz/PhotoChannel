using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IChannelCategoryService
    {
        IDataResult<List<Category>> GetChannelCategories(int channelId);
        IDataResult<List<Channel>> GetCategoryChannels(int categoryId);
        IDataResult<ChannelCategory> Add(ChannelCategory channelCategory);
        IResult AddRange(ChannelCategory[] channelCategories);
        IResult Delete(ChannelCategory channelCategory);
    }
}
