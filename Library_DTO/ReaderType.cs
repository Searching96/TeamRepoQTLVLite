using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library_DTO
{
    public class ReaderType
    {
        [Key]
        public int ReaderTypeID { get; set; }

        [Required]
        public string ReaderTypeName { get; set; }
    }
}
