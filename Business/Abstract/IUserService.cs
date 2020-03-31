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

        IDataResult<List<Photo>> GetPhotos(int id);
        IDataResult<List<OperationClaim>> GetClaims(int id);
        IDataResult<List<Like>> GetLikes(int id);
        IDataResult<List<ChannelAdmin>> GetChannelsManaged(int id);
        IDataResult<List<Subscriber>> GetSubscriptions(int id);

        IResult Delete(int id);
        IDataResult<User> Add(User user);
        IDataResult<User> Update(UserForUpdateDto userForUpdateDto, int id);
        IResult UserExists(string email);
        IResult UserExists(int id);
        IResult UserExistsWithUpdate(string email, int id);
    }
}
