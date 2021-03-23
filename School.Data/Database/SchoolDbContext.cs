using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;

namespace School.Data.Database
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Programme> Programmes { get; set; }
    }
}