using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class Contact
{
    public int ContactsId { get; set; }

    public int? ContactUserId { get; set; }

    public virtual Usr? ContactUser { get; set; }

    public virtual ICollection<Usr> Usrs { get; set; } = new List<Usr>();
}
