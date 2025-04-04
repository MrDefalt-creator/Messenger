using Microsoft.AspNetCore.Http.HttpResults;

namespace Backend.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("register", Register);
        app.MapPost("login", Login);

        return app;
    }

    private static async Task<IResult> Register()
    {
        return Results.Ok();
    }

    private static async Task<IResult> Login()
    {
        return Results.Ok();
    }
}