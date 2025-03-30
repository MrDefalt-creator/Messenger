using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Usr
{
    public int UsrId { get; set; }

    public string? Login { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<BlockList> BlockLists { get; set; } = new List<BlockList>();

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<BlockList> BlockListsNavigation { get; set; } = new List<BlockList>();

    public virtual ICollection<Chat> ChatsNavigation { get; set; } = new List<Chat>();

    public virtual ICollection<Contact> ContactsNavigation { get; set; } = new List<Contact>();
}
