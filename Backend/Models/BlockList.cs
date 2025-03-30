using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class BlockList
{
    public int BlockListId { get; set; }

    public int? BlockUserId { get; set; }

    public virtual Usr? BlockUser { get; set; }

    public virtual ICollection<Usr> Usrs { get; set; } = new List<Usr>();
}
