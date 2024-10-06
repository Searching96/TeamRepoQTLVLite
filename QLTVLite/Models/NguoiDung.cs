using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTVLite.Models
{
    public class NguoiDung
    {
        [Key]
        public int ID {  get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string MaNguoiDung {  get; set; }
        public string TenNguoiDung { get; set; }
        public string MatKhau { get; set; }
        public int PhanQuyen {  get; set; }
    }
}
