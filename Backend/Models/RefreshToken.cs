using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public partial class RefreshToken
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TokenId { get; set; }

    public int? UsrId { get; set; }

    public string? Token { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public bool? IsRevoked { get; set; } =  false;

    public virtual Usr? Usr { get; set; }
}
