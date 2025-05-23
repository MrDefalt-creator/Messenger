﻿using Backend.Contracts.User;
using Backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("register", Register);
        app.MapPost("login", Login);
        app.MapPost("jwt_update", UpdateJwt);

        return app;
    }

    private static async Task<IResult> Register([FromBody] UserRegisterRequest request, UserService service)
    {
        await service.Register(request.name, request.email, request.password);
        return Results.Ok();
    }

    private static async Task<IResult> Login([FromBody] UserLoginRequest request, UserService service)
    {
        var response = await service.Login(request.email, request.password, request.rememberMe);
        return Results.Ok(response);
    }

    private static async Task<IResult> UpdateJwt(UserService service)
    {
        var updatedToken =  await service.UpdateJwtToken();
        return Results.Ok(updatedToken);
    }
}