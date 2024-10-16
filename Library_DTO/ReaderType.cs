using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_DTO;

public partial class ReaderType
{
    [Key]
    public int ReaderTypeId { get; set; }

    public string? ReaderTypeName { get; set; }

    public virtual ICollection<Reader> Readers { get; set; } = new List<Reader>();
}
