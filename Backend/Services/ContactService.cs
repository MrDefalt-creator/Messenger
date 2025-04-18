using Backend.Configuration;
using Backend.DTOs;
using Backend.Interfaces.Auth;
using Backend.Interfaces.Repositories;
using Backend.Models;

namespace Backend.Services;

public class ContactService
{
    private readonly IGetUserFromClaims _getUserFromClaims;
    private readonly IUserRepository _userRepository;
    private readonly MessengerContext _dbContext;

    public ContactService(IGetUserFromClaims getUserFromClaims, IUserRepository userRepository, MessengerContext dbContext)
    {
        _getUserFromClaims = getUserFromClaims;
        _userRepository = userRepository;
        _dbContext = dbContext;
        
    }

    public async Task AddContact(int contactId)
    {
        var userId = _getUserFromClaims.UserFromClaimsFromHeaders();

        if (!await _userRepository.UserExists(contactId) || !await _userRepository.UserExists(userId))
        {
            throw new Exception("Ошибка добавления в контакты");
        }
        
        var user = await _userRepository.GetUserWithContactsById(userId);
        var contactUser = await _userRepository.GetUserWithContactsById(contactId);
        
        user.Contacts.Add(contactUser);
        
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task DeleteContact(int contactId)
    {
        var userId = _getUserFromClaims.UserFromClaimsFromHeaders();

        if (!await _userRepository.UserExists(contactId) || !await _userRepository.UserExists(userId))
        {
            throw new Exception("Ошибка удаления контакта");
        }
        var user = await _userRepository.GetUserWithContactsById(contactId);
        var contactUser = await _userRepository.GetUserWithContactsById(contactId);
        
        user.Contacts.Remove(contactUser);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<ContactDto>> GetContacts()
    {
        var userId = _getUserFromClaims.UserFromClaimsFromHeaders();

        if (!await _userRepository.UserExists(userId))
        {
            throw new Exception("Ошибка получения контактов");
        }
        
        var contacts = await _userRepository.GetContactsById(userId);
        
        return contacts;
        
    }

    public async Task BlockContact(int contactId)
    {
        var userId = _getUserFromClaims.UserFromClaimsFromHeaders();

        if (!await _userRepository.UserExists(contactId) || !await _userRepository.UserExists(userId))
        {
            throw new Exception("Ошибка блокировки контакта");
        }
        
        var user = await _userRepository.GetUserWithContactsById(userId);
        var contactUser = await _userRepository.GetUserWithContactsById(contactId);
        
        user.Contacts.Remove(contactUser);
        
        user.BlockedUsers.Add(contactUser);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task UnblockContact(int contactId)
    {
        var userId = _getUserFromClaims.UserFromClaimsFromHeaders();

        if (!await _userRepository.UserExists(contactId) || !await _userRepository.UserExists(userId))
        {
            throw new Exception("Ошибка разблокировки контакта");
        }
        
        var user = await _userRepository.GetUserWithContactsById(userId);
        var contactUser = await _userRepository.GetUserWithContactsById(contactId);
        
        user.BlockedUsers.Remove(contactUser);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<ContactDto>> GetBlockedContacts()
    {
        var userId = _getUserFromClaims.UserFromClaimsFromHeaders();

        if (!await _userRepository.UserExists(userId))
        {
            throw new Exception("Ошибка получения списка блокировки");
        }
        
        var blockedContacts = await _userRepository.GetBlockedContacts(userId);
        
        return blockedContacts;
        
    }
    
    
}