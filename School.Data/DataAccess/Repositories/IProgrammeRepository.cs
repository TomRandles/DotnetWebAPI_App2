
using School.Data.DataAccess.Repositories.Generic;
using School.Domain.Models;
using System.Threading.Tasks;

namespace School.Data.DataAccess.Repositories
{
    public interface IProgrammeRepository : IRepository<ProgrammeDto> 
    {
        Task DeleteProgrammeAsync(string Id);

        Task<ProgrammeDto> GetProgrammeWithStudents(string programmeID);
    }
}