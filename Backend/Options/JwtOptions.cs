namespace Backend.Options;

// Данные поля могут быть дополнены
public class JwtOptions
{
    public string SecretKey { get; set; } = "Backend_Messenger_SecretKey_For_JWT_Authorization";

    public int ExpiresIn { get; set; } = 2;
}