using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Endpoints;

public static class ContactsEndpoint
{
    public static IEndpointRouteBuilder MapContactsEndpoint(this IEndpointRouteBuilder endpoints)
    {
       var app = endpoints.MapGroup("/contacts").RequireAuthorization();
       
       app.MapPost("add_contact/{contactId}", AddContact);
       
       app.MapDelete("delete_contact/{contactId}", DeleteContact);
       
       app.MapGet("get_contacts", GetContacts);

       app.MapPost("block_contact/{contactId}", BlockContact);
       
       app.MapDelete("unblock_contact{contactId}", UnblockContact);
       
       app.MapGet("get_blocked_contacts", GetBlockedContacts);
       
       return app;
    }

    private static async Task<IResult> AddContact(ContactService service,[FromRoute] int contactId)
    {
        await service.AddContact(contactId);
        return Results.Ok();
    }

    private static async Task<IResult> DeleteContact(ContactService service, [FromRoute] int contactId)
    {
        await service.DeleteContact(contactId);
        return Results.Ok();
    }

    private static async Task<IResult> GetContacts(ContactService service)
    {
        var contacts = await service.GetContacts();
        return Results.Ok(contacts);
    }

    private static async Task<IResult> BlockContact(ContactService service, [FromRoute] int contactId)
    {
        await service.BlockContact(contactId);
        return Results.Ok();
    }

    private static async Task<IResult> UnblockContact(ContactService service, [FromRoute] int contactId)
    {
        await service.UnblockContact(contactId);
        return Results.Ok();
    }

    private static async Task<IResult> GetBlockedContacts(ContactService service)
    {
        var blockedContacts = await service.GetBlockedContacts();
        return Results.Ok(blockedContacts);
    }
}