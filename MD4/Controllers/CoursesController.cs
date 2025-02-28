using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MD4.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace MD4.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses
public async Task<IActionResult> Index()
{
    var courses = await _context.Course
        .Include(c => c.Teacher) // ieladejam pasniedzeja vardu uzvardu
        .ToListAsync();

    return View(courses);
}

        // GET: Courses/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewBag.Teachers = _context.Teacher
                .Select(t => new { t.Id, FullName = t.Name + " " + t.Surname })
                .ToList();
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int teacherId, string courseName)
        {
            string sql = "INSERT INTO Course (Name, TeacherId) VALUES (@courseName, @teacherId)";

            await _context.Database.ExecuteSqlRawAsync(sql,
                new[]
                {
                    new SqlParameter("@courseName", courseName),
                    new SqlParameter("@teacherId", teacherId)
                });

            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

    
            ViewBag.Teachers = _context.Teacher
                .Select(t => new { t.Id, FullName = t.Name + " " + t.Surname })
                .ToList();

            return View(course);
        }



        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int teacherId, string courseName)
        {
            if (id == 0)
            {
                return NotFound();
            }

            string sql = "UPDATE Course SET Name = @courseName, TeacherId = @teacherId WHERE Id = @id";

            await _context.Database.ExecuteSqlRawAsync(sql,
                new[]
                {
            new SqlParameter("@courseName", courseName),
            new SqlParameter("@teacherId", teacherId),
            new SqlParameter("@id", id)
                });

            return RedirectToAction(nameof(Index));
        }



        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
