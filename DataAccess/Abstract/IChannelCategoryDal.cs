using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IChannelCategoryDal : IEntityRepository<ChannelCategory>
    {
        List<Category> GetChannelCategories(Channel channel);
        List<Channel> GetCategoryChannels(Category category);
        void AddRange(ChannelCategory[] channelCategories);
    }
}
