using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoChannelWebAPI.Exceptions
{
    public class NoUserIdException : Exception
    {
        public NoUserIdException()
        {
            Message = "Unauthorization.User Id must not be null.";
        }

        public override string Message { get; }
    }
}
