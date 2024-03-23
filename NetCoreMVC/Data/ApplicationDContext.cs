using Microsoft.EntityFrameworkCore;
using NetCoreMVC.Models;

namespace NetCoreMVC.Data{
    public class ApplicationDbContext :DbContext{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Student>Student{ get; set; }

    }
}