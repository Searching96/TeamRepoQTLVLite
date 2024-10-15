using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_DTO
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
    }
}

