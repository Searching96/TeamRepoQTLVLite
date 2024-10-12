using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Library_DTO
{
    public class Book
    {
        public int BookID { get; set; }

        //[Required]
        //public string Title { get; set; }

        //[Required]
        //public string Author { get; set; }

        //public Image COVER { get; set; }

        //public string ISBN { get; set; }

        public bool isBorrowed { get; set; }

        //public override string ToString()
        //{
        //    return $"{BookID}: {Title} by {Author}";
        //}

    }
}

