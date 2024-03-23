using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreAPI.Data;
using NetCoreAPI.Models;

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
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            return await _Context.Student.ToListAsync();
        }
        [HttpGet("{studentid}")]
        public async Task<ActionResult<Student>> GetStudent(string studentid)
        {
            if (_Context.Student == null)
            {
                return NotFound();
            }
            var student = await _Context.Student.FindAsync(studentid);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }
        private bool CheckStudentid(string studentid)
        {
            return (_Context.Student?.Any(e => e.StudentID == studentid)).GetValueOrDefault();
        }
        [HttpPut("{studenid}")]
        public async Task<ActionResult<Student>> PutStudent(string studentid, Student student)
        {
            if (studentid != student.StudentID)
            {
                return BadRequest();
            }
            _Context.Entry(student).State = EntityState.Modified;
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
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            if (_Context.Student == null)
            {
                return Problem("Danh sách null");
            }
            _Context.Student.Add(student);
            try{
                await _Context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException){
            if(!CheckStudentid(student.StudentID)){
                return Conflict();
            }
            else{
                throw;
            }
            }
            return CreatedAtAction("GetStudent", new{studentid=student.StudentID},student);
        }
        [HttpDelete("{studentid}")]
        public async Task<IActionResult> DeleteStudent(string studentid){
             if (_Context.Student == null)
            {
                return NotFound();
            }
            var student = _Context.Student.Find(studentid);
            if(student == null){
                return NotFound();
            }
            _Context.Student.Remove(student);
            await _Context.SaveChangesAsync();
            return NoContent();
            }
        }

    }