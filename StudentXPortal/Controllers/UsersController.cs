using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentX.StudentXPortal.Data;
using StudentX.StudentXPortal.Models;
using Microsoft.EntityFrameworkCore;


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

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
            .Where(u => !u.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
            return View(users);
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

        [HttpGet("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var user = await _context.Users.FindAsync(id);  //FirstOrDefaultAsync yerine daha performanslı

            if (user == null)
            {
                return NotFound("Kullanıcı Bulunamadı");
            }

            return View(user);
        }

        [HttpPost("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedUser);
            }

            var user = await _context.Users.FindAsync(id);  //FirstOrDefaultAsync yerine daha performanslı


            if (user == null)
            {
                return NotFound("Kullanıcı Bulunamadı");
            }

            user.UserName = updatedUser.UserName;
            user.Password = updatedUser.Password;
            user.UserType = updatedUser.UserType;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Users");
        }


        [HttpPost("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted);

            if (user == null)
            {
                return NotFound("Kullanıcı Bulunamadı");
            }

            user.IsDeleted = true;
            user.DeletedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Users");
        }






        // API için yazdık
        // [HttpGet("GetUsers")]
        // public async Task<IActionResult> GetUsers()
        // {
        //     var users = await _context.Users.AsNoTracking().Where(i => !i.IsDeleted).ToListAsync();

        //     if (!users.Any())
        //     {
        //         return NotFound("Kullanıcılar bulunamadı");
        //     }


        //     return View(users);
        // }

        // [HttpGet("GetUserById/{id}")]
        // public async Task<IActionResult> GetUserById(int id)
        // {
        //     var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync((i => i.Id == id && !i.IsDeleted));

        //     if (user == null)
        //     {
        //         return NotFound("Kullanıcı Bulunamadı");
        //     }

        //     return View(user);
        // }

    }
}