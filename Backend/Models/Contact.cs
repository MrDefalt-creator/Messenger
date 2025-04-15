using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public partial class Contact
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContactsId { get; set; }

    public int? ContactUserId { get; set; }

    public virtual Usr? ContactUser { get; set; }

    public virtual ICollection<Usr> Usrs { get; set; } = new List<Usr>();
}
