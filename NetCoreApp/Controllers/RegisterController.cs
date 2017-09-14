using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Auction.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Auction.Controllers
{
    public class RegisterController : Controller
    {
        private AuctionContext _context;

        public RegisterController(AuctionContext context)
        {
            _context = context;
        }


        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult RegisterPage()
        {
            return View("Register");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult LoginPage()
        {
            ViewBag.NiceTry = TempData["NiceTry"];
            return View("Login");
        }
        
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }

//================================== Register and Login automatically =========================================
        [HttpPost]
        [Route("register")]
        public IActionResult Create(RegisterViewModel NewUser)
        {
            if (ModelState.IsValid)
            {
                // Check if Email is alredy regitered in DataBase
                List<User> CheckUsername = _context.Users.Where(theuser => theuser.Email == NewUser.Email).ToList();
                if (CheckUsername.Count > 0)
                {
                    ViewBag.ErrorRegister = "Username already in use...";
                    return View("Register");
                }
                // Password hashing
                PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                // Adding the created User Object to the DB
                User user = new User
                {
                    FirstName = NewUser.FirstName,
                    LastName = NewUser.LastName,
                    Email = NewUser.Email,
                    Password = NewUser.Password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                // Extracting the JustCreated user in order to obtain his ID and full name for storing in session
                User JustCreated = _context.Users.Single(theUser => theUser.Email == NewUser.Email);
                HttpContext.Session.SetInt32("UserId", (int)JustCreated.UserId);
                HttpContext.Session.SetString("UserName", (string)JustCreated.FirstName);
                return Redirect("/dashboard/1");
            }
            return View("Register");
        }

//============================================= Login a User ==================================================
        [HttpPost]
        [Route("loginNow")]
        public IActionResult Login(string LEmail = null, string Password = null)
        {
            // Checking if user inputs anything in the fields
            if(Password != null && LEmail != null)
            {
                // Checking if a User this provided Email exists in DB
                List<User> CheckUser = _context.Users.Where(theuser => theuser.Email == LEmail).ToList();
                if (CheckUser.Count > 0)
                {
                    System.Console.WriteLine("===========================");
                    // Checking if the password matches
                    var Hasher  = new PasswordHasher<User>();
                    if(0 != Hasher.VerifyHashedPassword(CheckUser[0], CheckUser[0].Password, Password))
                    {
                        // If the checks are validated than save his ID and Name in session and redirect to the Dashboard page
                        HttpContext.Session.SetInt32("UserId", (int)CheckUser[0].UserId);
                        HttpContext.Session.SetString("UserName", (string)CheckUser[0].FirstName);
                        return Redirect("/dashboard/1");
                    }
                }
            }
            // If check are not validated display an error
            ViewBag.ErrorLogin = "Invalid Login Data...";
            return View("Login");
        }        
        
// ============================================================================================================

    }
}
