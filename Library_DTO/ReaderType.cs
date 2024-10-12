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
        public int ReaderTypeID { get; }

        [Required]
        public string ReaderTypeName { get; set; }
    }
}
