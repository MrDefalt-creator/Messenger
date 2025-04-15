using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Endpoints;

public static class ContactsEndpoint
{
    public static IEndpointRouteBuilder MapContactsEndpoint(this IEndpointRouteBuilder endpoints)
    {
       var app = endpoints.MapGroup("/contacts").RequireAuthorization();
       
       app.MapPost("add_contact/{contactId}", AddContact);
       
       app.MapDelete("delete_contact", DeleteContact);
       
       app.MapGet("get_contacts", GetContacts);

       app.MapPost("block_contact", BlockContact);
       
       app.MapDelete("unblock_contact", UnblockContact);
       
       app.MapGet("get_blocked_contacts", GetBlockedContacts);
       
       return app;
    }

    private static async Task<IResult> AddContact(ContactService service,[FromRoute] int contactId)
    {
        await service.AddContact(contactId);
        return Results.Ok();
    }

    private static async Task<IResult> DeleteContact()
    {
        return Results.Ok();
    }

    private static async Task<IResult> GetContacts()
    {
        return Results.Ok();
    }

    private static async Task<IResult> BlockContact()
    {
        return Results.Ok();
    }

    private static async Task<IResult> UnblockContact()
    {
        return Results.Ok();
    }

    private static async Task<IResult> GetBlockedContacts()
    {
        return Results.Ok();
    }
}