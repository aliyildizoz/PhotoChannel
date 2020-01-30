using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class ErrorDataResult<T> : DataResultBase<T> where T : class
    {
        public ErrorDataResult(string message, T data) : base(message, false, data)
        {
        }

        public ErrorDataResult(T data) : base(false, data)
        {
        }
        public ErrorDataResult(string message) : base(message, false, null)
        {
        }
    }
}
