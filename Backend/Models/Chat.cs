using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Chat
{
    public int ChatId { get; set; }

    public string? ChatType { get; set; }

    public int? InitSenderId { get; set; }

    public virtual Usr? InitSender { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<Usr> Usrs { get; set; } = new List<Usr>();
}
