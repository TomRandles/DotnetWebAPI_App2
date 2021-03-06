using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using School.Domain.Models;
using School.Data.Database;
using School.Data.DataAccess.Repositories.Generic;
using School.Domain.Entities;

namespace School.Data.DataAccess.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(SchoolDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Student> FindByIdIncludeProgrammeAsync(string Id)
        {
            return await dbContext.Students
                                  .Include(m => m.Programme)
                                  .FirstOrDefaultAsync(m => m.StudentId == Id);
        }

        public async Task<IEnumerable<Student>> AllAsyncIncludeProgramme()
        {
            return await base.dbContext
                             .Students
                             .Include(m => m.Programme)
                             .ToListAsync();
        }

        public async Task DeleteStudentAsync(string Id)
        {
            var student = await base.FindByIdAsync(Id);
            if (student == null)
                throw new DataAccessException($"DeleteStudentAsync: student: {Id} not found");

                base.Delete(student);
        }
    }
}