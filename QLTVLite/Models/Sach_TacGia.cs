using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTVLite.Models
{
    public class Sach_TacGia
    {
        public int IDSach { get; set; }

        public Sach Sach { get; set; }

        public int IDTacGia { get; set; }

        public TacGia TacGia { get; set; }
    }
}
