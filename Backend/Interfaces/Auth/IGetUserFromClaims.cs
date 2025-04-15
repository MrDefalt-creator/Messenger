namespace Backend.Interfaces.Auth;

public interface IGetUserFromClaims
{
    int UserFromClaimsFromCookies();
    
    int UserFromClaimsFromHeaders();
}