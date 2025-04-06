using Backend.Configuration;
using Backend.Infrastructure;
using Backend.Interfaces.Auth;
using Backend.Interfaces.Repositories;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly MessengerContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    public UserService(IUserRepository userRepository, MessengerContext dbContext, IPasswordHasher passwordHasher, 
        IHttpContextAccessor httpContextAccessor, IJwtProvider jwtProvider, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
    }
    public async Task Register(string name, string email, string password)
    {
        if (await _userRepository.GetUserByEmail(email) != null)
        {
            throw new Exception($"Пользователь c данным {email} уже существует");
        }
        
        string hashedPassword = _passwordHasher.HashPassword(password);

        var user = new Usr
        {
            Login = name,
            Email = email,
            PasswordHash = hashedPassword,
            Status = null
            
        };
        
        await _dbContext.Usrs.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task Login(string email, string password, bool rememberMe)
    {
        var user = await _userRepository.GetUserByEmail(email);

        if (user == null)
        {
            throw new Exception($"Пользователь с данным {email} не найден");
        }

        if (_passwordHasher.VerifyHashedPassword(password, user.PasswordHash) == false)
        {
            throw new Exception($"Не правильный пароль");
        }

        if (rememberMe &&  !await _refreshTokenRepository.RefreshTokenExists(user.UsrId))
        {

            var refreshToken = new RefreshToken
            {
                UsrId = user.UsrId,
                Token = _jwtProvider.GenerateJwtRefreshToken(),
                
            }; 
            
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = rememberMe ? DateTime.UtcNow.AddDays(30) : null,
        };
        
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("JWT", _jwtProvider.GenerateJwtToken(user), cookieOptions);
        
        
    }
}