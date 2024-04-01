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
        public PhotoChannelContext Context { get; private set; }
        public EfChannelCategoryDal(PhotoChannelContext context) : base(context)
        {
            Context = context;
        }

        public List<Category> GetChannelCategories(Channel channel)
        {
            var categories = Context.ChannelCategories.Include(category => category.Category)
                .Where(category => category.ChannelId == channel.Id).Select(category => category.Category);
            return categories.ToList();
        }

        public List<Channel> GetCategoryChannels(Category category)
        {
            var channels = Context.ChannelCategories.Include(c => c.Channel)
                .Where(c => c.CategoryId == category.Id).Select(c => c.Channel);

            return channels.ToList();
        }

        public void AddRange(ChannelCategory[] channelCategories)
        {
            var result = Context.ChannelCategories.Where(category => category.ChannelId == channelCategories[0].ChannelId).ToList();
            Context.ChannelCategories.RemoveRange(result);
            Context.ChannelCategories.AddRange(channelCategories);
            Context.SaveChanges();
        }
    }
}
