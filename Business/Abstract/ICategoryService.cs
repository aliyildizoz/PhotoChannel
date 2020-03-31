using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<List<Category>> GetList();
        IDataResult<List<Channel>> GetChannels(int id);
        IResult Add(Category category);
        IResult Exists(int id);
    }
}
