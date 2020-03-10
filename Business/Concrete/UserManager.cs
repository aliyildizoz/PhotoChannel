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
            User user = _userDal.Get(u => u.Email == email);
            if (user == null)
            {
                return new ErrorDataResult<User>(user);
            }
            return new SuccessDataResult<User>(user);
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
            _userDal.Delete(user);
            return new SuccessResult();
        }

        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Add(User user)
        {
            _userDal.Add(user);
            _userDal.AddOperationClaim(user);
            _userDetailService.Add(new UserDetail { UserId = user.Id, SubscriptionCount = 0 });
            return new SuccessResult(Messages.UserRegistered);
        }

        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Update(UserForUpdateDto userForUpdateDto)
        {
            User user = _userDal.Get(u => u.Id == userForUpdateDto.Id);
            if (user != null)
            {
                user.FirstName = string.IsNullOrEmpty(userForUpdateDto.FirstName)
                    ? user.FirstName
                    : userForUpdateDto.FirstName;
                user.LastName = string.IsNullOrEmpty(userForUpdateDto.LastName)
                    ? user.LastName
                    : userForUpdateDto.LastName;
                user.Email = string.IsNullOrEmpty(userForUpdateDto.Email)
                    ? user.Email
                    : userForUpdateDto.Email;
                if (!string.IsNullOrEmpty(userForUpdateDto.Password))
                {
                    UserForPasswordDto userForPasswordDto = new UserForPasswordDto
                    {
                        Password = userForUpdateDto.Password
                    };
                    HashingHelper.CreatePasswordHash(userForPasswordDto);
                    user.PasswordHash = userForPasswordDto.PasswordHash;
                    user.PasswordSalt = userForPasswordDto.PasswordSalt;
                }
                _userDal.Update(user);
                return new SuccessResult();
            }

            return new ErrorResult(Messages.UserNotFound);
        }
        public IResult UserExists(string email)
        {
            IDataResult<User> result = GetByEmail(email);
            if (result.IsSuccessful)
            {
                return new SuccessResult(Messages.UserAlreadyExists);
            }
            return new ErrorResult();
        }
        public IResult UserExistsWithUpdate(string email, int userId)
        {
            IDataResult<User> result = GetByEmail(email);
            if (result.IsSuccessful)
            {
                return result.Data.Id == userId ? (IResult) new ErrorResult() : new SuccessResult(Messages.UserAlreadyExists);
            }
            return new ErrorResult();
        }
    }
}
