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
        IDataResult<List<PhotoCardDto>> GetPhotos(User user);

        IDataResult<List<OperationClaim>> GetClaims(User user);
        IDataResult<List<Channel>> GetSubscriptions(User user);
        IDataResult<List<Channel>> GetChannels(User user);
        IDataResult<List<PhotoCardDto>> GetLikedPhotos(User user);
        IResult Delete(User user);
        IDataResult<User> Add(User user);
        IDataResult<User> Update(UserForUpdateDto userForUpdateDto, int userId);
        IResult UserExists(string email);
        IResult UserExistsWithUpdate(string email, int userId);

    }
}
