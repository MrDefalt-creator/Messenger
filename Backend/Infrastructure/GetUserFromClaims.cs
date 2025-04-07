using Backend.Interfaces.Auth;

namespace Backend.Infrastructure;

public class GetUserFromClaims(IHttpContextAccessor httpContextAccessor) : IGetUserFromClaims
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
    public int UserFromClaims()
    {
        var claims = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

        if (claims == null)
        {
            throw new Exception("Куки не найдены");
        }
        
        return int.Parse(claims);
    }
}