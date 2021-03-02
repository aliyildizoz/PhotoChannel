using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryDal _categoryDal;
        private Validation<CategoryValidator> _validation;
        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public IDataResult<List<Category>> GetList()
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.GetList().ToList());
        }

        public IResult Add(Category category)
        {
            _validation = new Validation<CategoryValidator>();
            _validation.Validate(category);
            _categoryDal.Add(category);
            return new SuccessResult();
        }
        public bool Contains(IEntity entity)
        {
            return _categoryDal.Contains(new Category { Id = entity.Id });
        }
    }
}
