using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class File
{
    public int FileId { get; set; }

    public string? FileType { get; set; }

    public string? PathFile { get; set; }

    public string? FileName { get; set; }

    public DateTime? UploadedDate { get; set; }

    public int? UserId { get; set; }

    public virtual Usr? User { get; set; }

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public virtual ICollection<Message> Msgs { get; set; } = new List<Message>();
}
