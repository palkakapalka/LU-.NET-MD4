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
    public class SubmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubmissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Submissions
        public async Task<IActionResult> Index()
        {
            var submissions = await _context.Submission
                .Include(s => s.Assignment) // pievienojam assignment discription
                    .ThenInclude(a => a.Course) // pievienojam kursa vardu
                .Include(s => s.Student)    // pievienojam studenta FullName
                .Select(s => new
                {
                    s.Id,
                    AssignmentDescription = s.Assignment.Description, // assignmrnt discri[tion
                    CourseName = s.Assignment.Course.Name, // kursa nosaukums
                    StudentFullName = s.Student.Name + " " + s.Student.Surname, // studenta FullName
                    s.SubmissionTime,
                    s.Score
                })
                .ToListAsync();

            
            ViewBag.Submissions = submissions;

            return View(submissions);
        }


        [Authorize]
        // GET: Submissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submission = await _context.Submission
                .FirstOrDefaultAsync(m => m.Id == id);
            if (submission == null)
            {
                return NotFound();
            }

            return View(submission);
        }

        // GET: Submissions/Create
        public IActionResult Create()
        {
            // veidojam sarakstu ar kursa nosaukumiem
            ViewBag.Assignments = _context.Assignment
                .Include(a => a.Course)
                .Select(a => new
                {
                    a.Id,
                    AssignmentCourseDescription = a.Course.Name + ", " + a.Description
                })
                .ToList();

            // veidojam studenta sarakstu
            ViewBag.Students = _context.Students
                .Select(s => new
                {
                    s.Id,
                    FullName = s.Name + " " + s.Surname
                })
                .ToList();

            return View();
        }





        // POST: Submissions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int assignmentId, int studentId, DateTime submissionTime, decimal score)
        {
            if (ModelState.IsValid)
            {
                string sql = "INSERT INTO Submission (AssignmentId, StudentId, SubmissionTime, Score) " +
                             "VALUES (@assignmentId, @studentId, @submissionTime, @score)";

                await _context.Database.ExecuteSqlRawAsync(sql,
                    new[]
                    {
                new SqlParameter("@assignmentId", assignmentId),
                new SqlParameter("@studentId", studentId),
                new SqlParameter("@submissionTime", submissionTime),
                new SqlParameter("@score", score)
                    });

                return RedirectToAction(nameof(Index));
            }

            // Повторно загружаем списки при ошибке
            var assignments = _context.Assignment
                .Include(a => a.Course)
                .Select(a => new
                {
                    a.Id,
                    FullDescription = $"{a.Description} (Course: {a.Course.Name})"
                })
                .ToList();

            var students = _context.Students
                .Select(s => new
                {
                    s.Id,
                    FullName = $"{s.Name} {s.Surname}"
                })
                .ToList();

            ViewBag.Assignments = assignments;
            ViewBag.Students = students;

            return View();
        }



        // GET: Submissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submission = await _context.Submission.FindAsync(id);
            if (submission == null)
            {
                return NotFound();
            }

            // Формируем список заданий с курсами
            ViewBag.Assignments = _context.Assignment
                .Include(a => a.Course) // Подгружаем данные о курсах
                .Select(a => new
                {
                    Id = a.Id,
                    AssignmentCourseDescription = a.Course.Name + ", " + a.Description
                })
                .ToList();

            // Формируем список студентов
            ViewBag.Students = _context.Students
                .Select(s => new
                {
                    Id = s.Id,
                    FullName = s.Name + " " + s.Surname
                })
                .ToList();

            return View(submission);
        }

        // POST: Submissions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int assignmentId, int studentId, DateTime submissionTime, decimal score)
        {
            if (id == 0)
            {
                return NotFound();
            }

            string sql = @"
        UPDATE Submission 
        SET AssignmentId = @assignmentId, 
            StudentId = @studentId, 
            SubmissionTime = @submissionTime, 
            Score = @score 
        WHERE Id = @id";

            await _context.Database.ExecuteSqlRawAsync(sql,
                new[]
                {
            new SqlParameter("@assignmentId", assignmentId),
            new SqlParameter("@studentId", studentId),
            new SqlParameter("@submissionTime", submissionTime),
            new SqlParameter("@score", score),
            new SqlParameter("@id", id)
                });

            return RedirectToAction(nameof(Index));
        }





        // GET: Submissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submission = await _context.Submission
                .FirstOrDefaultAsync(m => m.Id == id);
            if (submission == null)
            {
                return NotFound();
            }

            return View(submission);
        }

        // POST: Submissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var submission = await _context.Submission.FindAsync(id);
            if (submission != null)
            {
                _context.Submission.Remove(submission);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubmissionExists(int id)
        {
            return _context.Submission.Any(e => e.Id == id);
        }
    }
}
