using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project1.Models{
    [Table("Students")]
    public class Student{
        [Key]
        public String StudentID { get; set; }
        public String Fullname { get; set; }
        public int  Age { get; set; }
        public String Address { get; set; }
    }
}