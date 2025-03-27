using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentXPortal.Models;

namespace StudentX.StudentXPortal.Controllers
{

    [Route("[controller]")]

    public class EducationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }




}