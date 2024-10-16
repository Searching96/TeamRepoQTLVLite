using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_DTO;

public partial class Book
{
    [Key]
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public bool? IsBorrowed { get; set; }

    public virtual ICollection<BorrowDetail> BorrowDetails { get; set; } = new List<BorrowDetail>();

    public virtual ICollection<ReturnDetail> ReturnDetails { get; set; } = new List<ReturnDetail>();
}
