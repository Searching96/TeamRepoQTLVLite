using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTVLite.Models
{
    public class TacGia
    {
        [Key]
        public int ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string MaTacGia { get; set; }
        public string TenTacGia { get; set; }
        public ICollection<Sach_TacGia> Sach_TacGia { get; set; }
    }
}
