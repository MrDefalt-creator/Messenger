using Backend.Configuration;
using Backend.Interfaces.Repositories;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MessengerContext _dbContext;

    public UserRepository(MessengerContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    async public Task<Usr?> GetUserById(int id)
    {
        return await _dbContext
            .Usrs
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UsrId == id);
    }

    async public Task<Usr?> GetUserByEmail(string email)
    {
        return await _dbContext
            .Usrs
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}