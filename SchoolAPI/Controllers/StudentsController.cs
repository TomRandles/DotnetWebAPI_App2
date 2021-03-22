﻿using Microsoft.AspNetCore.Mvc;
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
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            this._studentRepository = studentRepository;
        }
        // HTTP Get routes to GetStudents
        [HttpGet]
        public async Task<IActionResult> GetStudentsAsync()
        {
            var students = await _studentRepository.AllAsync();
            return Ok(students);
        }

        // GET: api/students/s50001
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudentAsync(string id)
        {
            var student = await _studentRepository.FindByIdAsync(id);
            if (student != null)
                return Ok(student);
            else
                return NotFound();
        }

        // PUT: api/Student/s50025
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Student object deserialized from json
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentAsync(string id, StudentDto student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }
            try
            {
                var result = _studentRepository.Update(student);
                if (result == null)
                    return NotFound();
                await _studentRepository.SaveChangesAsync();
            }
            catch (DataAccessException)
            {

            }
            return NoContent();
        }

        // POST: api/students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentDto>> PostStudentAsync(StudentDto student)
        {
            try
            {
                var addedStudent = await _studentRepository.AddAsync(student);
                await _studentRepository.SaveChangesAsync();
                return CreatedAtAction("GetStudent", new { id = addedStudent.StudentId }, addedStudent);
            }
            catch (DataAccessException)
            {
                return BadRequest("Post student failed.");
            }
        }

        // DELETE: api/Students/S355505
        [HttpDelete("{id}")]
        public async Task DeleteStudent(string id)
        {
            try
            {
                await _studentRepository.DeleteStudentAsync(id);
                await _studentRepository.SaveChangesAsync();
            }
            catch (DataAccessException e)
            {

            }
        }
    }
}