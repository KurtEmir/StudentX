using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentX.StudentXPortal.Data;
using StudentX.StudentXPortal.Models;
using StudentX.StudentXPortal.Models.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentX.StudentXPortal.Controllers
{
    [Route("[controller]")]
    public class QuestionsController : Controller
    {
        private readonly StudentXDbContext _context;
        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(StudentXDbContext context, ILogger<QuestionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        [HttpGet("CreateQuestion")]
        public async Task<IActionResult> CreateQuestion(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            ViewBag.SelectedCourseName = course.CourseName;
            return View(new CreateQuestionViewModel { CourseId = id });
        }

        [HttpPost("CreateQuestion")]
        public async Task<IActionResult> CreateQuestion(CreateQuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var course = await _context.Courses.FindAsync(model.CourseId);
                ViewBag.SelectedCourseName = course?.CourseName;
                return View(model);
            }

            var question = new Question
            {
                QuestionSentence = model.QuestionSentence,
                CourseId = model.CourseId,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Questions");
        }


        [HttpGet("UpdateQuestion/{id}")]
        public async Task<IActionResult> UpdateQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return NotFound("Soru bulunamadı.");

            ViewBag.Courses = await _context.Courses
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            return View(question);
        }

        [HttpPost("UpdateQuestion/{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, Question updatedQuestion)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = await _context.Courses
                    .Where(c => !c.IsDeleted)
                    .ToListAsync();

                return View(updatedQuestion);
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return NotFound("Soru bulunamadı.");

            question.QuestionSentence = updatedQuestion.QuestionSentence;
            question.CourseId = updatedQuestion.CourseId;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Questions");
        }

        [HttpPost("DeleteQuestion/{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
                return NotFound("Soru bulunamadı.");

            question.IsDeleted = true;
            question.DeletedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Questions");
        }
    }
}
