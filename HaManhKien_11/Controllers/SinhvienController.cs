using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaManhKien_11.Models;
using HaManhKien_11.Models.ViewModels;
using HaManhKien_11.Models.Process;

namespace HaManhKien_11.Controllers
{
    public class SinhvienController : Controller
    {
        private readonly LTQLDb _context;
        private ExcelProcess _excelprocess = new ExcelProcess();

        public SinhvienController(LTQLDb context)
        {
            _context = context;
        }

        // GET: Sinhvien
        public async Task<IActionResult> Index()
        {
            var lTQLDb = _context.SinhVien.Include(s => s.LopHoc);
            return View(await lTQLDb.ToListAsync());
        }

        // GET: Sinhvien/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhVien
                .Include(s => s.LopHoc)
                .FirstOrDefaultAsync(m => m.MaSinhVien == id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // GET: Sinhvien/Create
        public IActionResult Create()
        {
            ViewData["MaLop"] = new SelectList(_context.LopHoc, "MaLop", "MaLop");
            return View();
        }

        // POST: Sinhvien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSinhVien,HoTen,MaLop")] SinhVien sinhVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sinhVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLop"] = new SelectList(_context.LopHoc, "MaLop", "MaLop", sinhVien.MaLop);
            return View(sinhVien);
        }

        // GET: Sinhvien/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhVien.FindAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            ViewData["MaLop"] = new SelectList(_context.LopHoc, "MaLop", "MaLop", sinhVien.MaLop);
            return View(sinhVien);
        }

        // POST: Sinhvien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSinhVien,HoTen,MaLop")] SinhVien sinhVien)
        {
            if (id != sinhVien.MaSinhVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sinhVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SinhVienExists(sinhVien.MaSinhVien))
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
            ViewData["MaLop"] = new SelectList(_context.LopHoc, "MaLop", "MaLop", sinhVien.MaLop);
            return View(sinhVien);
        }

        // GET: Sinhvien/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhVien
                .Include(s => s.LopHoc)
                .FirstOrDefaultAsync(m => m.MaSinhVien == id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // POST: Sinhvien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sinhVien = await _context.SinhVien.FindAsync(id);
            if (sinhVien != null)
            {
                _context.SinhVien.Remove(sinhVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SinhVienExists(string id)
        {
            return _context.SinhVien.Any(e => e.MaSinhVien == id);
        }
        public async Task<IActionResult> Index2(int malop)
        {
            malop = 1;
            var query = (from std in _context.SinhVien
                         join lophoc in _context.LopHoc
                       on std.MaLop equals lophoc.MaLop
                         where lophoc.MaLop == malop
                         select new SinhVienVM
                         {
                             MaSinhVien = std.MaSinhVien,
                             HoTen = std.HoTen,
                             MaLop = lophoc.MaLop,
                             TenLop = lophoc.TenLop
                         }
                        ).ToList().Take(2);
            return View(query);
        }
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("File is required ");
            }
            var fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension.ToLower() != ".xlsx" & fileExtension.ToLower() != ".xls")
            {
                return BadRequest("file không đúng định dạng");
            }
            var fileName = file.FileName;
            var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName);
            var fileLocation = new FileInfo(filePath).ToString();
            var ExistingStudent = _context.SinhVien.Select(e => e.MaSinhVien).ToList();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                var dt = _excelprocess.ExcelToDataTable(fileLocation);
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    if (!ExistingStudent.Contains(dt.Rows[i][0]))
                    {
                        var sv = new SinhVien();
                        sv.MaSinhVien = dt.Rows[i][0].ToString();
                        sv.HoTen = dt.Rows[i][1].ToString();
                        _context.SinhVien.Add(sv);
                    }

                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
