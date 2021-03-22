using Microsoft.AspNetCore.Mvc;
using School.Data.DataAccess.Repositories;
using School.Data.DataAccess.Repositories.Generic;
using School.Domain.Models;
using System;
using System.Threading.Tasks;

namespace SchoolAPI.Controllers
{
    // ControllerBase - basic functionality: model state, current user, std  responses
    // Controller - not required for API - involves views
    // ApiController - improves API dev experience.
    [ApiController]
    // Common routing requirements for all actions
    [Route("api/programmes/")]
    public class ProgrammesController : ControllerBase
    {
        private readonly IProgrammeRepository _programmeRepository;

        public ProgrammesController(IProgrammeRepository programmeRepository)
        {
            this._programmeRepository = programmeRepository;
        }
        // HTTP Get routes to GetProgrammes
        [HttpGet]
        public async Task<IActionResult> GetProgrammesAsync()
        {
            var programmes = await _programmeRepository.AllAsync();
            return Ok(programmes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentsForProgramme(string id)
        {
            var programme = await _programmeRepository.GetProgrammeWithStudents(id);
            if (programme != null)
                return Ok(programme);
            else
                return NotFound();
        }

        // GET: api/Programmes/s50001
        public async Task<ActionResult<ProgrammeDto>> GetProgrammeAsync(string id)
        {
            var programme = await _programmeRepository.FindByIdAsync(id);
            if (programme != null)
                return Ok(programme);
            else
                return NotFound();
        }

        // PUT: api/programmes/s50025
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Programme object deserialized from json
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgrammeAsync(string id, ProgrammeDto programme)
        {
            if (id != programme.ProgrammeId)
            {
                return BadRequest();
            }

            try
            {
                var result = _programmeRepository.Update(programme);
                if (result == null)
                    return NotFound();
                await _programmeRepository.SaveChangesAsync();
            }
            catch (DataAccessException)
            {
                return BadRequest($"Update of programme {id} failed.");
            }
            return NoContent();
        }

        // POST: api/Programmes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProgrammeDto>> PostProgrammeAsync(ProgrammeDto programme)
        {
            try
            {
                var addedProgramme = await _programmeRepository.AddAsync(programme);
                await _programmeRepository.SaveChangesAsync();

                return CreatedAtAction("GetProgramme", new { id = addedProgramme.ProgrammeId }, addedProgramme);
            }
            catch (DataAccessException)
            {
                return BadRequest("Post programme failed.");
            }
        }

        // DELETE: api/programmes/S355505
        [HttpDelete("{id}")]
        public async Task DeleteProgrammeAsync(string id)
        {
            try
            {
                await _programmeRepository.DeleteProgrammeAsync(id);
                await _programmeRepository.SaveChangesAsync();
            }
            catch (DataAccessException)
            {
                BadRequest($"Delete of {id} failed");
            }
        }
    }
}