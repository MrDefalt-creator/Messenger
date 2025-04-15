using System.IdentityModel.Tokens.Jwt;
using Backend.Interfaces.Auth;

namespace Backend.Infrastructure;

public class GetUserFromClaims(IHttpContextAccessor httpContextAccessor) : IGetUserFromClaims
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
    public int UserFromClaimsFromCookies()
    {
        var claims = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

        if (claims == null)
        {
            throw new Exception("Куки не найдены");
        }
        
        return int.Parse(claims);
    }

    public int UserFromClaimsFromHeaders()
    {
        var claims = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(claims))
        {
            throw new Exception("Требуется авторизация");
        }

        var userId = new JwtSecurityTokenHandler().ReadJwtToken(claims).Claims.FirstOrDefault(c => c.Type == "userId");

        if (userId == null)
        {
            throw new Exception("Требуется авторизация");
        }
        
        return int.Parse(userId.Value);
        
    }
}