using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface ISearchDal
    {
        Tuple<List<User>, List<Channel>> SearchByText(string text);
        List<Channel> SearchByCategory(int categoryId);
        List<Channel> SearchByMultiCategory(int[] categoryIds);
    }
}
