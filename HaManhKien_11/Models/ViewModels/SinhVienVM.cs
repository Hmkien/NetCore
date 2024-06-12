using System.ComponentModel.DataAnnotations;

namespace HaManhKien_11.Models.ViewModels
{
    public class SinhVienVM
    {
        [Display(Name = "Mã sinh viên")]
        public string MaSinhVien { get; set; }
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; }
        [Display(Name = "Mã lớp")]
        public int MaLop { get; set; }
        [Display(Name = "Tên Lớp")]
        public string TenLop { get; set; }
    }
}