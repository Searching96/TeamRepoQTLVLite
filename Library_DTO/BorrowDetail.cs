using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DTO
{
    public class BorrowDetail
    {
        [Required]
        public int BorrowID { get; set; }

        [Required]
        public int BookID { get; set; }

        public DateOnly EndDate { get; set; }
    }
}
