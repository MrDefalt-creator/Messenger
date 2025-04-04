using System.ComponentModel.DataAnnotations;

namespace Backend.Contracts.User;

public record UserRegisterRequest(
    [Required] string name,
    [Required] string email,
    [Required] string password
    );