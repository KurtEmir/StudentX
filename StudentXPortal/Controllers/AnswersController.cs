using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentX.StudentXPortal.Data;
using StudentX.StudentXPortal.Models;
using StudentX.StudentXPortal.Models.ViewModels;


namespace StudentX.StudentXPortal.Controllers
{

    [Route("[controller]")]
    public class AnswersController : Controller
    {
        private readonly StudentXDbContext _context;
        private readonly ILogger<AnswersController> _logger;

        public AnswersController(StudentXDbContext context, ILogger<AnswersController> logger)
        {
            _context = context;
            _logger = logger;
        }




        [HttpGet("CreateAnswer/{questionId}")]
        public async Task<IActionResult> CreateAnswer(int questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null) return NotFound();

            // View'a Question bilgisi göstermek istersen ViewBag ile yolla
            ViewBag.QuestionSentence = question.QuestionSentence;

            return View(new CreateAnswerViewModel
            {
                QuestionId = questionId
            });
        }

        [HttpPost("CreateAnswer/{questionId}")]
        public async Task<IActionResult> CreateAnswer(int questionId, CreateAnswerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var question = await _context.Questions.FindAsync(questionId);
                ViewBag.QuestionSentence = question?.QuestionSentence;
                return View(model);
            }

            var courseId = await _context.Questions
                .Where(q => q.Id == questionId)
                .Select(q => q.CourseId)
                .FirstOrDefaultAsync();

            var answer = new Answer
            {
                QuestionId = model.QuestionId,
                Option = model.Option,
                IsCorrect = model.IsCorrect,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            // Cevap eklendikten sonra tekrar aynı kursun içerik listesine dönelim
            return RedirectToAction("SeeCourseContents", "Questions", new { id = courseId });
        }

        //View dosyası oluşturulacak
        [HttpGet("UpdateAnswer/{answerId}")]
        public async Task<IActionResult> UpdateAnswer(int answerId)
        {
            var answer = await _context.Answers
                .Include(a => a.Question)
                .FirstOrDefaultAsync(a => a.Id == answerId);

            if (answer == null) return NotFound();

            ViewBag.QuestionSentence = answer.Question?.QuestionSentence;
            ViewBag.AnswerId = answer.Id;

            return View(new CreateAnswerViewModel
            {
                QuestionId = answer.QuestionId,
                Option = answer.Option,
                IsCorrect = answer.IsCorrect
            });
        }


        [HttpPost("UpdateAnswer/{answerId}")]
        public async Task<IActionResult> UpdateAnswer(int answerId, CreateAnswerViewModel updatedAnswer)
        {

            if (!ModelState.IsValid)
            {
                return View(updatedAnswer);
            }

            var answer = await _context.Answers.FindAsync(answerId);

            if (answer == null)
            {
                return NotFound("Cevap bulunamadı.");
            }

            var courseId = await _context.Questions.AsNoTracking()
                .Where(q => q.Id == updatedAnswer.QuestionId)
                .Select(q => q.CourseId)
                .FirstOrDefaultAsync();



            answer.Option = updatedAnswer.Option;
            answer.IsCorrect = updatedAnswer.IsCorrect;

            await _context.SaveChangesAsync();

            return RedirectToAction("SeeCourseContents", "Questions", new { id = courseId });
        }








        [HttpPost("DeleteAnswer/{answerId}")]
        public async Task<IActionResult> DeleteAnswer(int answerId)
        {
            var answer = await _context.Answers
                .Include(a => a.Question)
                .FirstOrDefaultAsync(a => a.Id == answerId);

            if (answer == null)
            {
                return NotFound("Cevap bulunamadı.");
            }

            answer.IsDeleted = true;
            answer.DeletedDate = DateTime.UtcNow;

            int courseId = answer.Question.CourseId;

            await _context.SaveChangesAsync();

            return RedirectToAction("SeeCourseContents", "Questions", new { id = courseId });

        }












    }


}