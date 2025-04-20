using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentX.StudentXPortal.Data;
using StudentX.StudentXPortal.Models;

namespace StudentX.StudentXPortal.Controllers
{
    [Route("[controller]")]
    public class CoursesController : Controller
    {
        private readonly StudentXDbContext _context;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(StudentXDbContext context, ILogger<CoursesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Where(c => !c.IsDeleted)
                .AsNoTracking()
                .ToListAsync();

            return View(courses);
        }

        [HttpGet("CreateCourse")]
        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost("CreateCourse")]
        public async Task<IActionResult> CreateCourse(Course course)
        {
            course.CreatedDate = DateTime.UtcNow;
            course.IsDeleted = false;

            if (!ModelState.IsValid)
            {
                return View(course);
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Courses");
        }

        [HttpGet("UpdateCourse/{id}")]
        public async Task<IActionResult> UpdateCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound("Kurs bulunamadı.");
            }

            return View(course);
        }

        [HttpPost("UpdateCourse/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, Course updatedCourse)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedCourse);
            }

            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound("Kurs bulunamadı.");
            }

            course.CourseName = updatedCourse.CourseName;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Courses");
        }

        [HttpPost("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound("Kurs bulunamadı.");
            }

            course.IsDeleted = true;
            course.DeletedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Courses");
        }


    }
}
