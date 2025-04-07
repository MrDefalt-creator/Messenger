using System.ComponentModel.DataAnnotations;

namespace Backend.Contracts.User;

public record UserJwtUpdateRequest(
    [Required] int UserId,
    [Required] bool RememberMe
    );