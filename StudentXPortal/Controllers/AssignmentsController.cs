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
    public class AssignmentsController : Controller
    {
        private readonly StudentXDbContext _context;
        private readonly ILogger<AssignmentsController> _logger;

        public AssignmentsController(StudentXDbContext context, ILogger<AssignmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet("UsersAndCoursesList")]
        public async Task<IActionResult> UsersAndCoursesList()
        {
            var usersCourses = await _context.UserCourses
                .Include(uc => uc.User)
                .Include(uc => uc.Course)
                .Where(uc =>
                    !uc.IsDeleted &&
                    !uc.User.IsDeleted &&
                    !uc.Course.IsDeleted)
                .AsNoTracking()
                .ToListAsync();

            return View(usersCourses);
        }


        [HttpGet("AssignCourseToUser")]
        public async Task<IActionResult> AssignCourseToUser()
        {
            var courses = await _context.Courses
            .Where(c => !c.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

            var users = await _context.Users
            .Where(u => !u.IsDeleted && u.UserType == UserType.Ogrenci)
            .AsNoTracking()
            .ToListAsync();

            var viewModel = new AssignCourseToUserViewModel
            {
                Users = users,
                Courses = courses
            };

            return View(viewModel);
        }

        [HttpPost("AssignCourseToUser")]
        public async Task<IActionResult> AssignCourseToUser(AssignCourseToUserViewModel model)
        {
            var userCourse = new UserCourse
            {
                UserId = model.SelectedUserId,
                CourseId = model.SelectedCourseId,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.UserCourses.Add(userCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction("UsersAndCoursesList");
        }







    }


}