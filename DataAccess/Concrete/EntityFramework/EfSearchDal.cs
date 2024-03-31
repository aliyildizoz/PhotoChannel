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
    public class EfSearchDal : ISearchDal
    {
        public PhotoChannelContext Context { get; private set; }
        public EfSearchDal(PhotoChannelContext context)
        {
            Context = context;
        }
        public Tuple<List<User>, List<Channel>> SearchByText(string text)
        {
            Tuple<List<User>, List<Channel>> tuple = new Tuple<List<User>, List<Channel>>(
                Context.Users.Where(u => u.FirstName.Contains(text) || u.LastName.Contains(text) || u.UserName.Contains(text)).Take(5).ToList(),
                Context.Channels.Where(c => c.Name.Contains(text)).Take(5).ToList()
            );
            return tuple;
        }

        public List<Channel> SearchByCategory(int categoryId)
        {
            var result = Context.ChannelCategories.Include(c => c.Channel).Include(category => category.Channel.User).Where(c => c.CategoryId == categoryId).Select(c => c.Channel);

            return result.ToList();
        }

        public List<Channel> SearchByMultiCategory(int[] categoryIds)
        {
            var result = Context.ChannelCategories.Include(c => c.Channel).Include(category => category.Channel.User).Where(c => categoryIds.Contains(c.CategoryId.GetValueOrDefault())).Select(c => c.Channel).Distinct();

            return result.ToList();
        }
    }
}
