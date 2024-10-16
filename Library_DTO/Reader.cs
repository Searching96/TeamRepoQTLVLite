using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_DTO;

public partial class Reader
{
    [Key,ForeignKey("UsernameNavigation")]
    public string Username { get; set; } = null!;

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public int ReaderTypeId { get; set; } = 1;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int? CurrentBorrows { get; set; } = 0;

    public int? TotalDebt { get; set; } = 0;

    public virtual ReaderType ReaderType { get; set; } = null!;

    public virtual User UsernameNavigation { get; set; } = null!;

    //public Reader (string Username, int _readerTypeId = 1)
    //{
    //    ReaderTypeId = _readerTypeId;
    //    CurrentBorrows = 0;
    //    TotalDebt = 0;
    //}
}
