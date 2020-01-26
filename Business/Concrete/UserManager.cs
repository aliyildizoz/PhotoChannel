using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private IUserDal _userDal;
        private IUserDetailService _userDetailService;
        private IPhotoService _photoService;
        public UserManager(IUserDal userDal, IUserDetailService userDetailService, IPhotoService photoService)
        {
            _userDal = userDal;
            _userDetailService = userDetailService;
            _photoService = photoService;
        }
        public IDataResult<User> GetById(int id)
        {
            return new SuccessDataResult<User>(_userDal.Get(user => user.Id == id));
        }

        public IDataResult<UserDetail> GetUserDetailById(int id)
        {
            return _userDetailService.GetByUserId(id);
        }

        public IDataResult<List<User>> GetList()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetList().ToList());
        }

        public IDataResult<User> GetByEmail(string email)
        {
            return new SuccessDataResult<User>(_userDal.Get(user => user.Email == email));
        }
        public IDataResult<List<Photo>> GetPhotos(User user)
        {
            return _photoService.GetPhotosByUser(user);
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(user));
        }

        public IDataResult<List<Channel>> GetSubscriptions(User user)
        {
            return new SuccessDataResult<List<Channel>>(_userDal.GetSubscriptionList(user));
        }

        public IDataResult<List<Photo>> GetLikedPhotos(User user)
        {
            return new SuccessDataResult<List<Photo>>(_userDal.GetLikedPhotos(user));
        }
        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        public IDataResult<User> Delete(User user)
        {
            user.IsActive = false;
            _userDal.Delete(user);
            return new SuccessDataResult<User>(user);
        }
        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        public IDataResult<User> Add(User user)
        {
            _userDal.Add(user);
            return new SuccessDataResult<User>(Messages.UserRegistered, user);
        }
        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        public IDataResult<User> Update(User user)
        {
            _userDal.Update(user);
            return new SuccessDataResult<User>(user);
        }



    }
}
