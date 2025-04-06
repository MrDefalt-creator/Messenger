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
        return _dbContext.RefreshTokens.Any(rt => rt.UsrId == userId && rt.IsRevoked == false);
    }
}