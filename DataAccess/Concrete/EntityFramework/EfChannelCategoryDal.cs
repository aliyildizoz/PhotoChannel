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
    public class EfChannelCategoryDal : EfEntityRepositoryBase<ChannelCategory, PhotoChannelContext>, IChannelCategoryDal
    {
        public List<Category> GetChannelCategories(Channel channel)
        {
            using (var context = new PhotoChannelContext())
            {
                var categories = context.ChannelCategories.Include(category => category.Category)
                    .Where(category => category.ChannelId == channel.Id).Select(category => category.Category);
                return categories.ToList();
            }
        }

        public List<Channel> GetCategoryChannels(Category category)
        {
            using (var context = new PhotoChannelContext())
            {
                var channels = context.ChannelCategories.Include(c => c.Channel)
                    .Where(c => c.CategoryId == c.Id).Select(c => c.Channel);
                return channels.ToList();
            }
        }

        public void AddRange(ChannelCategory[] channelCategories)
        {
            using (var context = new PhotoChannelContext())
            {
                //Todo:conf
                context.ChannelCategories.AddRange(channelCategories);
            }
        }
    }
}
