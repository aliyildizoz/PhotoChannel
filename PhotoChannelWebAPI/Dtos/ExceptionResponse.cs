using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoChannelWebAPI.Dtos
{
    public class ExceptionResponse
    {
        public string Message { get; set; }

        public ExceptionResponse(string message)
        {
            Message = message;
        }
    }
}
