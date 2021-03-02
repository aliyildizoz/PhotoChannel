using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Entities.Concrete;
using Core.Utilities.Hashing;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private IUserDal _userDal;

        private Validation<UserValidator> _validation;
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public IDataResult<User> GetById(int id)
        {

            var user = _userDal.Get(u => u.Id == id);

            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>(Messages.UserNotFound);

        }
        public IDataResult<List<User>> GetList()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetList().ToList());
        }
        public IDataResult<User> GetByEmail(string email)
        {
            User user = _userDal.Get(u => u.Email == email);
            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>(Messages.UserNotFound);
        }

        public IDataResult<User> GetByRefreshToken(string refreshToken)
        {
            User user = _userDal.Get(u => u.RefreshToken.Contains(refreshToken));
            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>(Messages.UserNotFound);
        }

        public IDataResult<List<OperationClaim>> GetClaims(int id)
        {
            var result = Contains(new User { Id = id });
            if (!result)
            {
                return new ErrorDataResult<List<OperationClaim>>();
            }
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(new User { Id = id }));
        }

        public IResult Delete(int id)
        {
            _userDal.Delete(new User { Id = id });
            return new SuccessResult();
        }

        public IDataResult<User> Add(User user)
        {
            _validation = new Validation<UserValidator>();
            _validation.Validate(user);
            _userDal.Add(user);
            _userDal.AddOperationClaim(user);
            return new SuccessDataResult<User>(Messages.UserRegistered, user);
        }

        public IDataResult<User> Update(User user)
        {
            _validation = new Validation<UserValidator>();
            _validation.Validate(user);

            _userDal.Update(user);
            return new SuccessDataResult<User>(Messages.UserRegistered, user);
        }

        public IDataResult<User> UpdatePassword(User user, string password)
        {

            _validation = new Validation<UserValidator>();
            _validation.Validate(user);

            if (!string.IsNullOrEmpty(password))
            {
                UserForPasswordDto userForPasswordDto = new UserForPasswordDto
                {
                    Password = password
                };
                HashingHelper.CreatePasswordHash(userForPasswordDto);
                user.PasswordHash = userForPasswordDto.PasswordHash;
                user.PasswordSalt = userForPasswordDto.PasswordSalt;
                _userDal.Update(user);

                return new SuccessDataResult<User>(user);
            }

            return new ErrorDataResult<User>(Messages.PasswordIsNull, user);
        }

        public IDataResult<User> UpdateRefreshToken(User user)
        {
            User oldUser = _userDal.Get(u => u.Id == user.Id);
            if (oldUser != null)
            {
                _userDal.Update(user);
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>(Messages.UserNotFound);
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
            var user = _userDal.Get(u => u.Id != userId && u.Email == email);
            if (user != null)
            {
                return new SuccessResult(Messages.UserAlreadyExists);
            }
            return new ErrorResult();
        }

        public bool Contains(IEntity entity)
        {
            return _userDal.Contains(new User { Id = entity.Id });
        }
    }
}
