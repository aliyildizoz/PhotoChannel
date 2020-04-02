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
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        [CacheAspect]
        public IDataResult<User> GetById(int id)
        {
            var user = _userDal.Get(u => u.Id == id);
            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>();

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
            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>(Messages.UserNotFound);
        }

        [CacheAspect]
        public IDataResult<List<OperationClaim>> GetClaims(int id)
        {
            var result = UserExists(id);
            if (!result.IsSuccessful)
            {
                return new ErrorDataResult<List<OperationClaim>>(result.Message);
            }
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(new User { Id = id }));
        }

        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Delete(int id)
        {
            _userDal.Delete(new User { Id = id });
            return new SuccessResult();
        }

        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        [CacheRemoveAspect("IUserService.Get")]
        public IDataResult<User> Add(User user)
        {
            _userDal.Add(user);
            _userDal.AddOperationClaim(user);
            return new SuccessDataResult<User>(Messages.UserRegistered, user);
        }

        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        [CacheRemoveAspect("IUserService.Get")]
        public IDataResult<User> Update(UserForUpdateDto userForUpdateDto, int userId)
        {
            IDataResult<User> dataResult = GetById(userId);
            if (dataResult.IsSuccessful)
            {
                dataResult.Data.FirstName = string.IsNullOrEmpty(userForUpdateDto.FirstName)
                    ? dataResult.Data.FirstName
                    : userForUpdateDto.FirstName;
                dataResult.Data.LastName = string.IsNullOrEmpty(userForUpdateDto.LastName)
                    ? dataResult.Data.LastName
                    : userForUpdateDto.LastName;
                dataResult.Data.Email = string.IsNullOrEmpty(userForUpdateDto.Email)
                    ? dataResult.Data.Email
                    : userForUpdateDto.Email;
                if (!string.IsNullOrEmpty(userForUpdateDto.Password))
                {
                    UserForPasswordDto userForPasswordDto = new UserForPasswordDto
                    {
                        Password = userForUpdateDto.Password
                    };
                    HashingHelper.CreatePasswordHash(userForPasswordDto);
                    dataResult.Data.PasswordHash = userForPasswordDto.PasswordHash;
                    dataResult.Data.PasswordSalt = userForPasswordDto.PasswordSalt;
                }
                _userDal.Update(dataResult.Data);
                return new SuccessDataResult<User>(dataResult.Data);
            }

            return dataResult;
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

        public IResult UserExists(int id)
        {
            IDataResult<User> result = GetById(id);
            if (result.IsSuccessful)
            {
                return new SuccessResult(Messages.UserNotFound);
            }
            return new ErrorResult();
        }

        public IResult UserExistsWithUpdate(string email, int userId)
        {
            IDataResult<User> result = GetByEmail(email);
            if (result.IsSuccessful)
            {
                return result.Data.Id == userId ? (IResult)new ErrorResult() : new SuccessResult(Messages.UserAlreadyExists);
            }
            return new ErrorResult();
        }
    }
}
