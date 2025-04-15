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


    public async Task<bool> RefreshTokenExists(int userId)
    {
        return await _dbContext.RefreshTokens.AnyAsync(rt => rt.UsrId == userId && rt.IsRevoked == false);
    }

    public async Task<bool> RefreshTokenExists(string refreshToken)
    {
        return await _dbContext.RefreshTokens.AnyAsync(rt => rt.Token == refreshToken && rt.IsRevoked == false);
    }
    public async Task<RefreshToken?> GetRefreshToken(int userId)
    {
        return await _dbContext.
            RefreshTokens.
            AsNoTracking().
            FirstOrDefaultAsync(rt => rt.UsrId == userId && rt.IsRevoked == false);
    }
}