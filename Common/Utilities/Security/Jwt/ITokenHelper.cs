using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using User = Core.Entities.Concrete.User;

namespace Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);

    }
}
