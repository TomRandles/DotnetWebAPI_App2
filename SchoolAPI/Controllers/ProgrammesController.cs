using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using School.Data.DataAccess.Repositories;
using School.Data.DataAccess.Repositories.Generic;
using School.Domain.Entities;
using School.Domain.Models;
using System;
using System.Collections.Generic;
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
        private readonly ILogger<ProgrammesController> _logger;
        private readonly IMapper _mapper;

        public ProgrammesController(IProgrammeRepository programmeRepository,
                                    ILogger<ProgrammesController> logger,
                                    IMapper mapper)
        {
            this._programmeRepository = programmeRepository ?? throw new ArgumentNullException(nameof(_programmeRepository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(_logger));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        // HTTP Get routes to GetProgrammes
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgrammesAsync()
        {
            var programmes = await _programmeRepository.AllAsync();
            return Ok(_mapper.Map<IEnumerable<ProgrammeDto>>(programmes));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentsForProgramme(string id)
        {
            var programme = await _programmeRepository.GetProgrammeWithStudents(id);
            if (programme != null)
                // _mapper.Map
                return Ok(_mapper.Map<ProgrammeDto>(programme));
            else
            {
                _logger.LogWarning($"Get programme. Id {id} not found.");
                return NotFound();
            }
        }

        // GET: api/Programmes/s50001
        public async Task<ActionResult<ProgrammeDto>> GetProgrammeAsync(string id)
        {
            var programme = await _programmeRepository.FindByIdAsync(id);
            if (programme != null)
               
                return Ok(_mapper.Map<ProgrammeDto>(programme));
            else
            {
                _logger.LogWarning($"Get programme. Id {id} not found.");
                return NotFound();
            }
        }

        // PUT: api/programmes/s50025
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Programme object deserialized from json
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgrammeAsync(string id,
                                                           [FromBody] ProgrammeDto programme)
        {
            if (id != programme.ProgrammeId)
            {
                _logger.LogWarning($"Put programme. Id {id} does not match {programme.ProgrammeId}.");
                return BadRequest();
            }

            try
            {
                var programmeFromDb = await _programmeRepository.FindByIdAsync(id);
                if (programmeFromDb == null)
                {
                    _logger.LogWarning($"Get programme. Id {id} not found.");
                    return NotFound($"Programme {id} not found");
                }

                // Map all properties except Id
                programmeFromDb.Name = programme.Name;
                programmeFromDb.Description = programme.Description;

                var result = _programmeRepository.Update(programmeFromDb);
                if (result == null)
                {
                    _logger.LogWarning($"Put programme. Id {id} not found.");
                    return NotFound();
                }
                await _programmeRepository.SaveChangesAsync();
            }
            catch (DataAccessException e)
            {
                _logger.LogCritical(e, "Update failed.");
                return BadRequest($"Update of programme {id} failed.");
            }
            return NoContent();
        }

        // PATCH: api/programmes/Java06
        // Programme object deserialized from json
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateProgrammeAsync(string id,
            [FromBody] JsonPatchDocument<ProgrammeDto> patchProgramme)
        {
            try
            {
                var programmeFromDb = await _programmeRepository.FindByIdAsync(id);
                if (programmeFromDb == null)
                    return NotFound($"Programme {id} not found");

                var programmeToPatch = new ProgrammeDto
                {
                    Name = programmeFromDb.Name,
                    Description = programmeFromDb.Description
                };

                patchProgramme.ApplyTo(programmeToPatch, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!TryValidateModel(patchProgramme))
                {
                    return BadRequest(ModelState);
                }

                // Map all properties except Id
                programmeFromDb.Name = programmeToPatch.Name;
                programmeFromDb.Description = programmeToPatch.Description;

                var result = _programmeRepository.Update(programmeFromDb);
                if (result == null)
                {
                    _logger.LogError($"Programme ${programmeFromDb.ProgrammeId} update failed.");
                    return NotFound();
                }
                await _programmeRepository.SaveChangesAsync();
            }
            catch (DataAccessException e)
            {
                _logger.LogCritical(e, "Partial update failed.");
                return BadRequest($"Update of programme {id} failed.");
            }
            return NoContent();
        }

        // POST: api/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProgrammeDto>> PostProgrammeAsync([FromBody] ProgrammeDto programme)
        {
            if (programme.Description == programme.Name)
            {
                // Business rule - Name and Description must be different
                ModelState.AddModelError(programme.Description,
                    $"Name: {programme.Name} and description: {programme.Description} should be different.");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Partial update failed.");
                return BadRequest(ModelState);
            }

            var programmeToDb = _mapper.Map<Programme>(programme);

            try
            {
                var addedProgramme = await _programmeRepository.AddAsync(programmeToDb);
                await _programmeRepository.SaveChangesAsync();

                var programmeToReturn = _mapper.Map<ProgrammeDto>(addedProgramme);

                return CreatedAtAction("GetProgramme", new { id = addedProgramme.ProgrammeId }, programmeToReturn);
            }
            catch (DataAccessException e)
            {
                _logger.LogError(e, "Post programme failed.");
                return BadRequest("Post programme failed.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Post programme failed.");
                return BadRequest("Post programme failed.");
            }
        }

        // DELETE: api/programmes/S355505
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgrammeAsync(string id)
        {
            try
            {
                var programmeFromDb = await _programmeRepository.FindByIdAsync(id);
                if (programmeFromDb == null)
                    return NotFound($"Programme {id} not found");

                await _programmeRepository.DeleteProgrammeAsync(id);
                await _programmeRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (DataAccessException e)
            {
                _logger.LogError(e, $"Delete programme {id} failed.");
                return BadRequest($"Delete of {id} failed");
            }
        }
    }
}