using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_DTO
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password{ get; set; }

        [Required]
        public string DisplayName { get; set; }
    }
}

