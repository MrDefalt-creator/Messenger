using Backend.Configuration;
using Backend.Interfaces.Repositories;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Backend.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly MessengerContext _dbContext;

    public RefreshTokenRepository(MessengerContext dbContext)
    {
        _dbContext = dbContext;
    }


    async public Task<bool> RefreshTokenExists(int userId)
    {
        return _dbContext.RefreshTokens.Any(rt => rt.UsrId == userId && rt.IsRevoked == false);
    }

    async public Task<bool> RefreshTokenExists(string refreshToken)
    {
        return _dbContext.RefreshTokens.Any(rt => rt.Token == refreshToken && rt.IsRevoked == false);
    }
    async public Task<RefreshToken?> GetRefreshToken(int userId)
    {
        return await _dbContext.
            RefreshTokens.
            AsNoTracking().
            FirstOrDefaultAsync(rt => rt.UsrId == userId && rt.IsRevoked == false);
    }
}