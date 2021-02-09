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
        public Tuple<List<User>, List<Channel>> SearchByText(string text)
        {
            using (var context = new PhotoChannelContext())
            {
                Tuple<List<User>, List<Channel>> tuple = new Tuple<List<User>, List<Channel>>(
                    context.Users.Where(u => u.FirstName.Contains(text) || u.LastName.Contains(text) || u.UserName.Contains(text)).Take(5).ToList(),
                    context.Channels.Where(c => c.Name.Contains(text)).Take(5).ToList()
                );
                return tuple;
            }
        }

        public List<Channel> SearchByCategory(int categoryId)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.ChannelCategories.Include(c => c.Channel).Include(category => category.Channel.User).Where(c => c.CategoryId == categoryId).Select(c => c.Channel);

                return result.ToList();
            }
        }

        public List<Channel> SearchByMultiCategory(int[] categoryIds)
        {
            using (var context = new PhotoChannelContext())
            {
                var result = context.ChannelCategories.Include(c => c.Channel).Include(category => category.Channel.User).Where(c => categoryIds.Contains(c.CategoryId)).Select(c => c.Channel).Distinct();

                return result.ToList();
            }
        }
    }
}
