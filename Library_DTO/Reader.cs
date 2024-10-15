using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DTO
{
    public class Reader
    {
        [Key]
        public string Username { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        [Required]
        public int ReaderTypeID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CurrentBorrows { get; set; }

        public int TotalDebt { get; set; }
    }
}
