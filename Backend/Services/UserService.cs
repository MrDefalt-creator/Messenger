using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend.Configuration;
using Backend.Interfaces.Auth;
using Backend.Interfaces.Repositories;
using Backend.Models;


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

    public async Task<string> Login(string email, string password, bool rememberMe)
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
                Token = _jwtProvider.GenerateJwtRefreshToken(user),
                
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
        
        var userRefreshToken = await _refreshTokenRepository.GetRefreshToken(user.UsrId);
        if (userRefreshToken == null)
        {
            throw new Exception("Внутренняя ошибка");
        }
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("JWTRefresh", userRefreshToken.Token, cookieOptions);
        return _jwtProvider.GenerateJwtToken(user);

    }

    public async Task<string> UpdateJwtToken()
    {
        var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["JWTRefresh"];
        if (refreshToken == null)
        {
            throw new Exception("Требуется авторизация");
        }
        if (await _refreshTokenRepository.RefreshTokenExists(refreshToken))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(refreshToken);
            
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId");
            if (userIdClaim == null)
            {
                throw new Exception("Не удалось извлечь userId");
            }
            int userId = int.Parse(userIdClaim.Value);
            
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new Exception("Данного пользователя не существует");
            }
            
            var newJwtToken = _jwtProvider.GenerateJwtToken(user);

            return newJwtToken;
        }
        else
        {
            throw new Exception("Недействительный refresh token");
        }
        
         
    }
}