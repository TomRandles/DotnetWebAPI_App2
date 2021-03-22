using School.Data.DataAccess.Repositories.Generic;
using School.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace School.Data.DataAccess.Repositories
{
    public interface IStudentRepository : IRepository<StudentDto> 
    {
        Task<IEnumerable<StudentDto>> AllAsyncIncludeProgramme();
        Task<StudentDto> FindByIdIncludeProgrammeAsync(string Id);
        Task DeleteStudentAsync(string Id);
    }
}