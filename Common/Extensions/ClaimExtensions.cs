using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Extensions
{
    public static class ClaimExtensions
    {
        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            claims.Add(new Claim(ClaimTypes.Email, email));
        }
        public static void AddFirstName(this ICollection<Claim> claims, string firstName)
        {
            claims.Add(new Claim(CustomClaimTypes.FirstName, firstName));
        }
        public static void AddLastName(this ICollection<Claim> claims, string lastName)
        {
            claims.Add(new Claim(CustomClaimTypes.LastName, lastName));
        }
        public static void AddUserName(this ICollection<Claim> claims, string userName)
        {
            claims.Add(new Claim(CustomClaimTypes.UserName, userName));
        }
        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }
        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
    }
}
