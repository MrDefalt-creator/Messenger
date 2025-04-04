using System.ComponentModel.DataAnnotations;

namespace Backend.Contracts.User;

public record UserLoginRequest(
    [Required]string email,
    [Required]string password,
    [Required]bool rememberMe
    );