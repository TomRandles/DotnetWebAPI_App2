using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Domain.Models;

namespace School.Data.Database
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }
        public DbSet<StudentDto> Students { get; set; }
        public DbSet<ProgrammeDto> Programmes { get; set; }
    }
}