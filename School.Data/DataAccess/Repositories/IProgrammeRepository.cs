using School.Data.DataAccess.Repositories.Generic;
using School.Domain.Entities;
using System.Threading.Tasks;

namespace School.Data.DataAccess.Repositories
{
    public interface IProgrammeRepository : IRepository<Programme> 
    {
        Task DeleteProgrammeAsync(string Id);

        Task<Programme> GetProgrammeWithStudents(string programmeID);
    }
}