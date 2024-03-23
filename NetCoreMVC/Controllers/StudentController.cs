using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreMVC.Data;
using NetCoreMVC.Models;

namespace NetCoreMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _Context;
        public StudentController(ApplicationDbContext context)
        {
            _Context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _Context.Student.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("StudentID,Fullname,Age,Address")] Student student)
        {
            if (ModelState.IsValid)
            {
                var student2 = await _Context.Student.FindAsync(student.StudentID);
                if (student2!=null)
                {
                    ViewBag.message = "Sinh vien da ton tai";
                }
                else
                {
                    _Context.Student.Add(student);
                    await _Context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(student);
        }
        public async Task<IActionResult> Details(string studentid)
        {
            if (studentid == null)
            {
                return NotFound();
            }
            var student = await _Context.Student.FindAsync(studentid);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        public async Task<IActionResult> Edit(string studentid)
        {
            if (studentid == null)
            {
                return NotFound();
            }
            var student = await _Context.Student.FindAsync(studentid);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string studentid, [Bind("StudentID,Fullname,Age,Address")] Student student)
        {
            if (studentid != student.StudentID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _Context.Student.Update(student);
                    await _Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckStudentid(student.StudentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);

        }
        private bool CheckStudentid(string studentid)
        {
            return (_Context.Student?.Any(e => e.StudentID == studentid)).GetValueOrDefault();
        }
        public async Task<IActionResult> Delete(string studentid)
        {
            if (studentid == null)
            {
                return NotFound();
            }
            var student = await _Context.Student.FindAsync(studentid);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string studentid)
        {
            if (_Context.Student == null)
            {
                return Problem("Danh sách rỗng");
            }
            var student = await _Context.Student.FindAsync(studentid);
            if (student != null)
            {
                _Context.Student.Remove(student);
                _Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
    }

}