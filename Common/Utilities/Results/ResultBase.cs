using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class ResultBase : IResult
    {
        public ResultBase(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
        public ResultBase(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
        
        public bool IsSuccessful { get; }
        public string Message { get; }
    }
}
