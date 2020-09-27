using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PhotoChannelWebAPI.CustomActionResults
{
    public  class ServerErrorObjectResult: ObjectResult
    {
        public ServerErrorObjectResult([ActionResultObjectValue] object value) : base(value)
        {
        }
    }
}
