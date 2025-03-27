using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentX.StudentXPortal.Data;
using StudentX.StudentXPortal.Models;

namespace StudentX.StudentXPortal.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly StudentXDbContext _context;

        public UsersController(ILogger<UsersController> logger, StudentXDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("CreateUser")]
        public IActionResult CreateUser()
        {
            return View(); // Views/Users/CreateUser.cshtml açılır
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(User user)
        {
            user.CreatedDate = DateTime.UtcNow;
            user.IsDeleted = false;

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"FIELD: {state.Key} - ERROR: {error.ErrorMessage}");
                    }
                }

                return View(user); // valid değilse aynı sayfayı verileriyle döner
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Users");
        }



        [NonAction]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}