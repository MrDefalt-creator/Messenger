using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class RefreshToken
{
    public int TokenId { get; set; }

    public int? UsrId { get; set; }

    public string? Token { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsRevoked { get; set; }

    public virtual Usr? Usr { get; set; }
}
