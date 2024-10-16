using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_DTO;

public partial class Return
{
    public int ReturnId { get; set; }

    [ForeignKey("UsernameNavigation")]
    public string Username { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual User UsernameNavigation { get; set; } = null!;
}
