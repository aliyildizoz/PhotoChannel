using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<User>> GetList();
        IDataResult<User> GetById(int id);
        IDataResult<User> GetByEmail(string email);
        IDataResult<User> GetByRefreshToken(string refreshToken);
        bool Contains(User user);

        IDataResult<List<OperationClaim>> GetClaims(int id);

        IResult Delete(int id);
        IDataResult<User> Add(User user);
        IDataResult<User> Update(User user);
        IDataResult<User> UpdatePassword(User user, string password);
        IDataResult<User> UpdateRefreshToken(User user);
        IResult UserExists(string email);
        IResult UserExistsWithUpdate(string email, int id);
    }
}
