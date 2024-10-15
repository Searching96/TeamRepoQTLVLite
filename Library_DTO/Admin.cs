using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_DTO
{
    public class Admin
    {
        [Key]
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
