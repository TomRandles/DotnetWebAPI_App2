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
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IProgrammeRepository _programmeRepository;
        private readonly ILogger<StudentsController> _logger;
        private readonly IMapper _mapper;

        public StudentsController(IStudentRepository studentRepository,
                                  IProgrammeRepository programmeRepository,
                                  ILogger<StudentsController> logger,
                                  IMapper mapper)
        {
            this._studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(_studentRepository));
            this._programmeRepository = programmeRepository ?? throw new ArgumentNullException(nameof(_programmeRepository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(_logger));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }
        // HTTP Get routes to GetStudents
        [HttpGet]
        public async Task<IActionResult> GetStudentsAsync()
        {
            _logger.LogInformation("Get students");

            var students = await _studentRepository.AllAsync();
            return Ok(_mapper.Map<IEnumerable<StudentDto>>(students));
        }

        // GET: api/students/s50001
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudentAsync(string id)
        {
            _logger.LogInformation($"Get student {id}");
            var student = await _studentRepository.FindByIdAsync(id);
            if (student != null)
                return Ok(_mapper.Map<StudentDto>(student));
            else
            {
                _logger.LogWarning($"Get student. {id} not found.");
                return NotFound();
            }
        }

        // PUT: api/Student/s50025
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Student object deserialized from json
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentAsync(string id, [FromBody] StudentDto student)
        {
            if (id != student.StudentId)
            {
                return BadRequest($"{id} and student ID {student.StudentId} not equal.");
            }
            try
            {
                var studentFromDb = await _studentRepository.FindByIdAsync(id);
                if (studentFromDb == null)
                    return NotFound($"Student: {id} not found");

                var studentToDb = _mapper.Map<Student>(student);

                var result = _studentRepository.Update(studentToDb);
                if (result == null)
                    return NotFound();
                await _studentRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (DataAccessException e)
            {
                _logger.LogError(e, $"Db error.");
                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Db error.");
                return BadRequest();
            }
        }

        // PATCH: api/programmes/Java06
        // Programme object deserialized from json
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateStudentAsync(string id,
            [FromBody] JsonPatchDocument<StudentDto> patchStudent)
        {
            try
            {
                var studentFromDb = await _studentRepository.FindByIdAsync(id);
                if (studentFromDb == null)
                    return NotFound($"Student {id} not found");

                //var programmeToPatch = new ProgrammeDto
                //{
                //    Name = programmeFromDb.Name,
                //    Description = programmeFromDb.Description
                //};

                var studentToPatch = _mapper.Map<StudentDto>(studentFromDb);

                patchStudent.ApplyTo(studentToPatch, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!TryValidateModel(patchStudent))
                {
                    return BadRequest(ModelState);
                }

                // Map all properties except Id
                studentFromDb.Name = studentToPatch.Name;
                studentFromDb.Description = studentToPatch.Description;

                var result = _studentRepository.Update(studentFromDb);
                if (result == null)
                {
                    _logger.LogError($"Student ${studentFromDb.StudentId} update failed.");
                    return NotFound();
                }
                await _studentRepository.SaveChangesAsync();
            }
            catch (DataAccessException e)
            {
                _logger.LogCritical(e, "Partial update failed.");
                return BadRequest($"Partial update of student {id} failed.");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Partial update failed.");
                return BadRequest($"Partial update of student {id} failed.");
            }
            return NoContent();
        }

        // POST: api/students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentDto>> PostStudentAsync([FromBody] StudentDto student)
        {
            // Api automatically returns BadRequest if StudentDto == null
            try
            {
                // Check if programme exists 
                var programme = _programmeRepository.FindByIdAsync(student.ProgrammeId);
                if (programme == null)
                {
                    _logger.LogWarning($"Programme : {student.ProgrammeId} not found.");
                    return NotFound($"Programmee {student.ProgrammeId} does not exist");
                }

                // map StudentDto to Student
                var studentToDb = _mapper.Map<Student>(student);

                var addedStudent = await _studentRepository.AddAsync(studentToDb);
                await _studentRepository.SaveChangesAsync();

                var studentToReturn = _mapper.Map<StudentDto>(addedStudent);
                return CreatedAtAction("GetStudent", new { id = addedStudent.StudentId }, studentToReturn);
            }
            catch (DataAccessException e)
            {
                _logger.LogError(e, "Db error.");
                return BadRequest("Post student failed.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Db error.");
                return BadRequest("Post student failed.");
            }
        }

        // DELETE: api/Students/S355505
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            try
            {
                await _studentRepository.DeleteStudentAsync(id);
                await _studentRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (DataAccessException e)
            {
                _logger.LogCritical(e, $"Delete student {id} failed.");
                return BadRequest($"Delete of {id} failed");
            }
        }
    }
}