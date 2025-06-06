﻿using Backend.DTOs;
using Backend.Models;

namespace Backend.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<Usr?> GetUserById(int id);
    
    public Task<Usr?> GetUserByEmail(string email);
    
    public Task<bool> UserExists(int userId);
    
    public Task<Usr?> GetUserWithContactsById(int userId);
    
    public Task<List<ContactDto>> GetContactsById(int userId);
    
    public Task<List<ContactDto>> GetBlockedContacts(int userId);
}