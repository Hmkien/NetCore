using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        public StudentController(ApplicationDbContext context)
        {
            _Context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _Context.Student.ToListAsync();
        }
        [HttpGet("{studentid}")]
        public async Task<ActionResult<Student>> GetStudent(string studentid)
        {
            var student = await _Context.Student.FindAsync(studentid);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpPut("{studentid}")]
        public async Task<IActionResult> PutStudent(string studentid, Student student)
        {
            if (studentid != student.StudentID)
            {
                return BadRequest();
            }

            var existingStudent = await _Context.Student.FindAsync(studentid);
            if (existingStudent == null)
            {
                return NotFound();
            }

            _Context.Entry(existingStudent).CurrentValues.SetValues(student);

            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckStudentid(studentid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool CheckStudentid(string studentid)
        {
            return (_Context.Student?.Any(e => e.StudentID == studentid)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _Context.Student.Add(student);
            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }

            return CreatedAtAction("GetStudent", new { studentid = student.StudentID }, student);
        }

        [HttpDelete("{studentid}")]
        public async Task<IActionResult> DeleteStudent(string studentid)
        {
            var student = await _Context.Student.FindAsync(studentid);
            if (student == null)
            {
                return NotFound();
            }

            _Context.Student.Remove(student);
            await _Context.SaveChangesAsync();

            return NoContent();
        }
    }
}
