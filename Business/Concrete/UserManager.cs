using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Hashing;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

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

        [CacheAspect]
        public IDataResult<User> GetById(int id)
        {
            return new SuccessDataResult<User>(_userDal.Get(user => user.Id == id));
        }

        [CacheAspect]
        public IDataResult<UserDetail> GetUserDetailById(int id)
        {
            return _userDetailService.GetByUserId(id);
        }
        [CacheAspect]
        public IDataResult<List<User>> GetList()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetList().ToList());
        }

        [CacheAspect]
        public IDataResult<User> GetByEmail(string email)
        {
            return new SuccessDataResult<User>(_userDal.Get(user => user.Email == email));
        }

        [CacheAspect]
        public IDataResult<List<Photo>> GetPhotos(User user)
        {
            return _photoService.GetPhotosByUser(user);
        }

        [CacheAspect]
        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(user));
        }

        [CacheAspect]
        public IDataResult<List<Channel>> GetSubscriptions(User user)
        {
            return new SuccessDataResult<List<Channel>>(_userDal.GetSubscriptionList(user));
        }

        [CacheAspect]
        public IDataResult<List<Photo>> GetLikedPhotos(User user)
        {
            return new SuccessDataResult<List<Photo>>(_userDal.GetLikedPhotos(user));
        }

        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Delete(User user)
        {
            user.IsActive = false;
            _userDal.Delete(user);
            return new SuccessResult();
        }

        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Add(User user)
        {
            _userDal.Add(user);
            return new SuccessResult(Messages.UserRegistered);
        }

        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Update(UserForUpdateDto userForUpdateDto)
        {
            UserForPasswordDto userForPasswordDto = new UserForPasswordDto
            {
                Password = userForUpdateDto.Password
            };
            HashingHelper.CreatePasswordHash(userForPasswordDto);
            User user = new User
            {
                Id = userForUpdateDto.Id,
                Email = userForUpdateDto.Email,
                FirstName = userForUpdateDto.FirstName,
                LastName = userForUpdateDto.LastName,
                PasswordHash = userForPasswordDto.PasswordHash,
                PasswordSalt = userForPasswordDto.PasswordSalt
            };
            _userDal.Update(user);
            return new SuccessResult();
        }



    }
}
