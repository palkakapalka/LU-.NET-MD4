using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MD4.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace MD4.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        // GET: Assignments
        public async Task<IActionResult> Index()
        {
            var assignments = await _context.Assignment
                .Include(a => a.Course) // ieladejam kkursa datus
                .Select(a => new
                {
                    a.Id,
                    a.Deadline,
                    CourseName = a.Course.Name, // kursa nosaukums
                    a.Description
                })
                .ToListAsync();

            // padodam datus ViewBag 
            ViewBag.Assignments = assignments;

            return View(assignments);
        }


        // GET: Assignments/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // GET: Assignments/Create
        public IActionResult Create()
        {
            ViewBag.Courses = _context.Course
                .Select(c => new { c.Id, c.Name }) // 
                .ToList();
            return View();
        }


        // POST: Assignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int courseId, DateTime deadline, string description)
        {
            string sql = "INSERT INTO Assignment (CourseId, Deadline, Description) VALUES (@courseId, @deadline, @description)";

            await _context.Database.ExecuteSqlRawAsync(sql,
                new[]
                {
            new SqlParameter("@courseId", courseId),
            new SqlParameter("@deadline", deadline),
            new SqlParameter("@description", description)
                });

            return RedirectToAction(nameof(Index));
        }


        // GET: Assignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string sqlAssignment = "SELECT * FROM Assignment WHERE Id = @id";
            var assignment = await _context.Assignment
                .FromSqlRaw(sqlAssignment, new SqlParameter("@id", id))
                .FirstOrDefaultAsync();

            if (assignment == null)
            {
                return NotFound();
            }

            ViewBag.Courses = _context.Course
                .Select(c => new { c.Id, c.Name })
                .ToList();

            return View(assignment);
        }


        // POST: Assignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int courseId, DateTime deadline, string description)
        {
            if (id == 0)
            {
                return NotFound();
            }

            string sql = "UPDATE Assignment SET CourseId = @courseId, Deadline = @deadline, Description = @description WHERE Id = @id";

            await _context.Database.ExecuteSqlRawAsync(sql,
                new[]
                {
            new SqlParameter("@courseId", courseId),
            new SqlParameter("@deadline", deadline),
            new SqlParameter("@description", description),
            new SqlParameter("@id", id)
                });

            return RedirectToAction(nameof(Index));
        }


        // GET: Assignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment != null)
            {
                _context.Assignment.Remove(assignment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignment.Any(e => e.Id == id);
        }
    }
}
