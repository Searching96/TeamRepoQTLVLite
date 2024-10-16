using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_DTO;

public partial class ReturnDetail
{
    [Key]
    public int ReturnId { get; set; }

    [ForeignKey("BookId")]
    public int BookId { get; set; }

    public DateTime ReturnDate { get; set; }

    public string? Note { get; set; }

    public virtual Book Book { get; set; } = null!;
}
