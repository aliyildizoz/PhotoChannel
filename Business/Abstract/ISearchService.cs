using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ISearchService
    {
        IDataResult<Tuple<List<User>, List<Channel>>> SearchByText(string text);
        IDataResult<List<Channel>> SearchByCategory(int categoryId);
        IDataResult<List<Channel>> SearchByMultiCategory(int[] categoryIds);
    }
}
