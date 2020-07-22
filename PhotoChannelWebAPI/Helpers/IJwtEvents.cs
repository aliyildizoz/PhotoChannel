using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PhotoChannelWebAPI.Helpers
{
    internal interface IJwtEvents
    {
        Task TokenValidated(TokenValidatedContext context);
    }
}