using Backend.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace Backend.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    public Task<bool> RefreshTokenExists(int userId);
    
    public Task<bool> RefreshTokenExists(string refreshToken);
    
    public Task<RefreshToken?> GetRefreshToken(int userId);
}