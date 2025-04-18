using Backend.Configuration;
using Backend.DTOs;
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
    
    public async Task<Usr?> GetUserById(int id)
    {
        return await _dbContext
            .Usrs
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UsrId == id);
    }

    public async Task<Usr?> GetUserByEmail(string email)
    {
        return await _dbContext
            .Usrs
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> UserExists(int userId)
    {
        return await _dbContext
            .Usrs
            .AsNoTracking()
            .AnyAsync(u => u.UsrId == userId);
    }

    public async Task<Usr?> GetUserWithContactsById(int userId)
    {
        return await _dbContext
            .Usrs
            .Include(u => u.Contacts)
            .FirstOrDefaultAsync(u => u.UsrId == userId);
    }

    public async Task<List<ContactDto>> GetContactsById(int userId)
    {
        var user = await _dbContext
            .Usrs
            .Include(u => u.Contacts)
            .FirstOrDefaultAsync(u => u.UsrId == userId);
        
        return user?.Contacts.Select(c => new ContactDto 
            { 
                UsrId = c.UsrId, 
                Login = c.Login 
            }).ToList() ?? new List<ContactDto>();
    }

    public async Task<List<ContactDto>> GetBlockedContacts(int userId)
    {
        var user = await _dbContext
            .Usrs
            .Include(u => u.BlockedUsers)
            .FirstOrDefaultAsync(u => u.UsrId == userId);
        
        return user?.BlockedUsers.Select(c => new ContactDto
        {
            UsrId = c.UsrId,
            Login = c.Login
        }).ToList() ?? new List<ContactDto>();
    }
}