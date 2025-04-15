using Backend.Configuration;
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

        if (!await _userRepository.UserExists(contactId))
        {
            throw new Exception("Добавляемый в контакты пользователь не найден");
        }
        
        var user = await _userRepository.GetUserById(userId);
        var contactUser = await _userRepository.GetUserById(contactId);
        /* Необходимо исправить не правильно создаются записи в таблицах, разобраться как работать
        через EF core со связью многие ко многим без явной модели в БД */
        var contact = new Contact
        {
            ContactUser = user,
            Usrs = new List<Usr> { contactUser }
        };
        
        await _dbContext.Contacts.AddAsync(contact);
        await _dbContext.SaveChangesAsync();

    }

    public async Task DeleteContact()
    {
        
    }

    public async Task GetContacts()
    {
        
    }

    public async Task BlockContact()
    {
        
    }

    public async Task UnblockContact()
    {
        
    }
    
    
}