using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library_DTO
{
    public class ReturnDetail
    {
        [Required]
        public int ReturnID { get; set; }

        [Required]
        public int BookID { get; set; }

        public DateOnly ReturnDate { get; set; }

        public string Note { get; set; }
    }
}
