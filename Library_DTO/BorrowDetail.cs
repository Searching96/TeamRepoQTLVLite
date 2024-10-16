using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_DTO;

public partial class BorrowDetail
{
    [Key]
    public int BorrowId { get; set; }

    [ForeignKey("BookId")]
    public int BookId { get; set; }

    public DateTime EndDate { get; set; }

    public virtual Book Book { get; set; } = null!;
}
