using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DTO
{
    public class Return
    {
        [Required]
        public int ReturnID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public DateOnly Date { get; set; }
    }
}
