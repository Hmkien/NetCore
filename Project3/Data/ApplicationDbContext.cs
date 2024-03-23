using Microsoft.EntityFrameworkCore;
using Project1.Models;

namespace Project1.Data{
    public class ApplicationDbContext :DbContext{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
        public DbSet<Student>Student{get; set; }=default!;
    }
}