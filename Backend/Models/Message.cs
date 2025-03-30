using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Message
{
    public int MsgId { get; set; }

    public string? MsgText { get; set; }

    public DateTime? MsgTimestamp { get; set; }

    public int? SenderId { get; set; }

    public int? ChatId { get; set; }

    public virtual Chat? Chat { get; set; }

    public virtual Usr? Sender { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();
}
