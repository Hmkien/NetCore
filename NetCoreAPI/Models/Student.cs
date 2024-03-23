using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI.Models{
      [Table("Tbl_Student")]
    public class Student{
        [Key]
        public string StudentID { get; set; }
        public String Fullname { get; set; }
        public String Address { get; set; }
        public int Age  { get; set; }
    }
}