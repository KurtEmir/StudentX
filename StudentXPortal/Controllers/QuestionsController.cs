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
            var courses = await _context.Courses
                           .Where(c => !c.IsDeleted)
                           .AsNoTracking()
                           .ToListAsync();

            return View(courses);
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
                return NotFound("İçerik bulunamadı.");

            var viewModel = new UpdateQuestionViewModel
            {
                QuestionSentence = question.QuestionSentence
            };

            return View(viewModel);
        }

        [HttpPost("UpdateQuestion/{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, UpdateQuestionViewModel updatedQuestion)
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
                return NotFound("İçerik bulunamadı.");

            question.QuestionSentence = updatedQuestion.QuestionSentence;

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

        [HttpGet("SeeCourseContents/{id}")]
        public async Task<IActionResult> SeeCourseContents(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            ViewBag.courseId = course.Id;


            var contentList = await _context.Questions
            .Include(q => q.Answers)
            .Where(q => q.CourseId == id && !q.IsDeleted)
            .ToListAsync();

            if (contentList == null || !contentList.Any())
                return NotFound("İçerik bulunamadı.");

            return View(contentList);
        }

    }
}
