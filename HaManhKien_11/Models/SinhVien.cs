
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaManhKien_11.Models
{
    public class SinhVien
    {
        [Key]
        [Display(Name = "Mã sinh viên")]
        [MaxLength(20)]
        public string MaSinhVien { get; set; }

        [Display(Name = "Họ và tên")]
        [MaxLength(50)]
        public string HoTen { get; set; }

        [Display(Name = "Mã lớp")]
        public int? MaLop { get; set; }

        [ForeignKey("MaLop")]
        [Display(Name = "Mã lớp")]
        public LopHoc? LopHoc { get; set; }
    }
}