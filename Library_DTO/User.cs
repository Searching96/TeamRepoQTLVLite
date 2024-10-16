using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_DTO;

public partial class User
{
    [Key]
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string TypeOfUser { get; set; } = null!;

    public virtual Admin? Admin { get; set; }

    public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();

    public virtual Reader? Reader { get; set; }

    public virtual ICollection<Return> Returns { get; set; } = new List<Return>();
}
