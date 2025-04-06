using Backend.Models;

namespace Backend.Interfaces.Auth;

public interface IJwtProvider
{
    string GenerateJwtToken(Usr user);
    
    string GenerateJwtRefreshToken();
}