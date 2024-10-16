using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_DTO   ;

public partial class Admin
{
    [Key,ForeignKey("UsernameNavigation")]
    public string Username { get; set; } = null!;

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public virtual User UsernameNavigation { get; set; } = null!;
}
