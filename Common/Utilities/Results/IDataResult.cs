using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public interface IDataResult<T>
    {
        T Data { get; }
        string Message { get; }
        bool IsSuccessful { get; }
    }
}
