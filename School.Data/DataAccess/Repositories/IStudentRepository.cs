using School.Data.DataAccess.Repositories.Generic;
using School.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Data.DataAccess.Repositories
{
    public interface IStudentRepository : IRepository<Student> 
    {
        Task<IEnumerable<Student>> AllAsyncIncludeProgramme();
        Task<Student> FindByIdIncludeProgrammeAsync(string Id);
        Task DeleteStudentAsync(string Id);
    }
}