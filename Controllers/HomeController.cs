using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Final_Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace Final_Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _context;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }




        public IActionResult Index()
        {
             HttpContext.Session.Clear();
            return View();
        }




        [HttpPost("user/create")]
        public IActionResult CreateUser(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(l => l.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Dashboard");
            } else {
                return View("Index");
            }
        }



        [HttpPost("user/login")]
        public IActionResult LoginUser(Login_User LoggedIn)
        {
            if(ModelState.IsValid)
            {
                User userInDb = _context.Users.FirstOrDefault(l => l.Email == LoggedIn.Login_Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Login_Email", "Invalid Email or Password");
                    return View("Index");
                }
                PasswordHasher<Login_User> hasher = new PasswordHasher<Login_User>();

                var result = hasher.VerifyHashedPassword(LoggedIn, userInDb.Password, LoggedIn.Login_Password);

                if (result == 0)
                {
                    ModelState.AddModelError("Login_Eamil", "Invalid Email or Password!");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Dashboard");
            }
            else 
            {
                return View("Index");
            }
        }




        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
             if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Login_User = _context.Users.FirstOrDefault(l => l.UserId == HttpContext.Session.GetInt32("UserId"));
            ViewBag.AllMeet_Ups = _context.Meet_Ups.Include(l => l.Event_Coordinator).ToList();
            ViewBag.AllMeet_Ups = _context.Meet_Ups.Include(l => l.Users_Already_Joined).OrderBy(m => m.Date_and_Time).ToList();
            return View("Dashboard");
        }

        
        [HttpGet("/add/Meet_Up")]
        public IActionResult AddMeet_Up()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            return View("Create_Meet_Up");
        }




        [HttpPost("create/Meet_Up")]
        public IActionResult Create_Meet_Up(Meet_Up newMeet_Up)
        {
            if(ModelState.IsValid)
            {
                newMeet_Up.UserId = (int)HttpContext.Session.GetInt32("UserId");
                _context.Meet_Ups.Add(newMeet_Up);
                _context.SaveChanges();
                return RedirectToAction("ViewMeet_Up", new {Meet_UpId = newMeet_Up.Meet_UpId});
            } else {
                return View("Create_Meet_Up");
            }
        }


        
        [HttpGet("view/{Meet_UpId}")]
        public IActionResult ViewMeet_Up(int Meet_UpId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("View_Meet_Up");
            }
            ViewBag.AllMeet_Ups = _context.Meet_Ups.Include(l => l.Event_Coordinator).ToList();
            ViewBag.AllMeet_Ups = _context.Meet_Ups.FirstOrDefault(l => l.Meet_UpId == Meet_UpId);
            List<Friend> Friends = _context.Friends.Where(l => l.Meet_UpId == Meet_UpId).Include(l => l.User).ToList();
            ViewBag.Friends = Friends;
            return View("View_Meet_Up");
        }
        
        
        [HttpGet("join/{Meet_UpId}")]
        public IActionResult join(int Meet_UpId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            _context.Friends.Add(new Friend{UserId = (int)HttpContext.Session.GetInt32("UserId"), Meet_UpId = Meet_UpId});
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }



        [HttpGet("leave/{Meet_UpId}")]
        public IActionResult leave(int Meet_UpId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            var Guest = _context.Friends.FirstOrDefault(l => l.UserId == (int)HttpContext.Session.GetInt32("UserId") && l.Meet_UpId == Meet_UpId);
            _context.Friends.Remove(Guest);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        

        [HttpGet("delete/{Meet_UpId}")]
        public IActionResult Delete(int Meet_UpId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            var Wedding = _context.Meet_Ups.FirstOrDefault(l => l.Meet_UpId == Meet_UpId);
            _context.Meet_Ups.Remove(Wedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
