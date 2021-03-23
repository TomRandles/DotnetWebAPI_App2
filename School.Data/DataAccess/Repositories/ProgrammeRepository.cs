using School.Data.DataAccess.Repositories.Generic;
using School.Domain.Models;
using School.Data.Database;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace School.Data.DataAccess.Repositories
{
    public class ProgrammeRepository : GenericRepository<Programme>, IProgrammeRepository 
    {
        public ProgrammeRepository(SchoolDbContext dbContext) : base (dbContext)
        { 
        }
        public async Task DeleteProgrammeAsync(string Id)
        {
            var programme = await base.FindByIdAsync(Id);
            if (programme != null)
                base.Delete(programme);
            else
                throw new DataAccessException($"DeleteProgrammeAsync: programme: {Id} not found");
        }

        public async Task<Programme> GetProgrammeWithStudents(string programmeID)
        {
            var programme = await dbContext.Programmes.Where(p => p.ProgrammeId == programmeID)
                                           .Include(s => s.Students).FirstOrDefaultAsync();
            return programme;
        }
    }
}