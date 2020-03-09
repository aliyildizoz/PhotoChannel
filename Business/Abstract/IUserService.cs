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
        IDataResult<User> GetById(int id);
        IDataResult<UserDetail> GetUserDetailById(int id);
        IDataResult<List<User>> GetList();
        IDataResult<User> GetByEmail(string email);
        IDataResult<List<Photo>> GetPhotos(User user);

        IDataResult<List<OperationClaim>> GetClaims(User user);
        IDataResult<List<Channel>> GetSubscriptions(User user);
        IDataResult<List<Photo>> GetLikedPhotos(User user);
        IResult Delete(User user);
        IResult Add(User user);
        IResult Update(UserForUpdateDto userForUpdateDto);
        IResult UserExists(string email);
        IResult UserExistsWithUpdate(string email, int userId);

    }
}
