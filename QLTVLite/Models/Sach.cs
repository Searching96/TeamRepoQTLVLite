using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTVLite.Models
{
    public class Sach
    {
        // tai sao thuoc tinh lai public, co phai vi internal?
        // quay lai day
        [Key]
        public string MaSach { get; set; }
        public string TenSach {  get; set; }
        public string TacGia { get; set; }
        public string TheLoai { get; set; }
        public int NamXuatBan {  get; set; }
    }
}
