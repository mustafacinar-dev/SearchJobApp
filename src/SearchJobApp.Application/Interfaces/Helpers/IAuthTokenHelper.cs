using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SearchJobApp.Application.Interfaces.Helpers;

public interface IAuthTokenHelper
{
    JwtSecurityToken ValidateToken(string token);
    string GenerateToken(IEnumerable<Claim> claims);
}