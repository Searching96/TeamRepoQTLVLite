using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLTVLite.Models
{
    public class Sach
    {
        // tai sao thuoc tinh lai public, co phai vi internal?
        // quay lai day 
        [Key]
        public int ID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string MaSach { get; set; }
        public string TenSach {  get; set; }
        public string TheLoai { get; set; }
        public int NamXuatBan {  get; set; }
        public string BiaSach { get; set; }

        [NotMapped]
        public string DSTacGia { get; set; }
        public ICollection<Sach_TacGia> Sach_TacGia { get; set; }
    }
}
