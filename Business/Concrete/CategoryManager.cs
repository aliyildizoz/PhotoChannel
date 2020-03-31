using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        [CacheAspect(duration: 10000)]
        public IDataResult<List<Category>> GetList()
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.GetList().ToList());
        }

        public IDataResult<List<Channel>> GetChannels(int id)
        {
            var result = Exists(id);
            if (result.IsSuccessful)
            {
                return new ErrorDataResult<List<Channel>>(result.Message);
            }
            return new SuccessDataResult<List<Channel>>(_categoryDal.GetChannels(new Channel { Id = id }));
        }

        [CacheRemoveAspect("ICategoryService.Get")]
        public IResult Add(Category category)
        {
            _categoryDal.Add(category);
            return new SuccessResult();
        }

        public IResult Exists(int id)
        {
            var category = _categoryDal.Get(c => c.Id == id);
            if (category != null)
            {
                return new SuccessResult();
            }
            return new ErrorResult(Messages.CategoryNotFound);
        }
    }
}
