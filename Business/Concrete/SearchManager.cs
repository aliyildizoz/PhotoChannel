using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class SearchManager : ISearchService
    {
        private ISearchDal _searchDal;

        public SearchManager(ISearchDal searchDal)
        {
            _searchDal = searchDal;
        }

        public IDataResult<Tuple<List<User>, List<Channel>>> SearchByText(string text)
        {
            Tuple<List<User>, List<Channel>> result = _searchDal.SearchByText(text);
            if (result.Item1.Count == 0 && result.Item2.Count == 0)
            {
                return new ErrorDataResult<Tuple<List<User>, List<Channel>>>(Messages.SearchNotFound, result);
            }
            return new SuccessDataResult<Tuple<List<User>, List<Channel>>>(result);
        }

        public IDataResult<List<Channel>> SearchByCategory(int categoryId)
        {
            var result = _searchDal.SearchByCategory(categoryId);
            if (result.Count == 0)
            {
                return new ErrorDataResult<List<Channel>>(Messages.ChannelNotFound, result);
            }
            return new SuccessDataResult<List<Channel>>(result);
        }

        public IDataResult<List<Channel>> SearchByMultiCategory(int[] categoryIds)
        {
            var result = _searchDal.SearchByMultiCategory(categoryIds);
            if (result.Count == 0)
            {
                return new ErrorDataResult<List<Channel>>(Messages.ChannelNotFound, result);
            }
            return new SuccessDataResult<List<Channel>>(result);
        }
    }
}
