using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public interface IResult
    {
        bool IsSuccessful { get; }
        string Message { get; }
    }
}
