using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wall.Models;
using Microsoft.EntityFrameworkCore;


namespace Wall.Controllers
{
    public class HomeController : Controller
    {
        private WallContext _context;
    
        public HomeController(WallContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index(Users myUser)
        {
            return View("Index");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(Validations validator)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<Validations> Hasher = new PasswordHasher<Validations>();
                validator.password = Hasher.HashPassword(validator, validator.password);
                Users myUser = new Users();
                myUser.first_name = validator.first_name;
                myUser.last_name = validator.last_name;
                myUser.email = validator.email;
                myUser.password = validator.password;
                myUser.created_at = DateTime.Now;
                myUser.updated_at = DateTime.Now;
                _context.Add(myUser);
                _context.SaveChanges();

                HttpContext.Session.SetInt32("UserID", myUser.id);
                int? UserID = HttpContext.Session.GetInt32("UserID");
                ViewBag.UserID = UserID;
                return RedirectToAction("Wall");
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet("Wall")]
        public IActionResult Wall()
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            ViewBag.userInfo = _context.users.Where(p => p.id == UserID).SingleOrDefault();
            ViewBag.allMessages = _context.messages.Include(p => p.Users).OrderByDescending(p => p.created_at).ToList();
            List<Comments> allComments = _context.comments.Include(p => p.Users).ToList();
            ViewBag.allComments = allComments;
            return View("Wall");
        }
        [HttpPost("LoginProcess")]
        public IActionResult LoginProcess(Logins myLogin)
        {
        if(ModelState.IsValid)
            {
                Users loginData = _context.users.SingleOrDefault(p => p.email == myLogin.email);
                if(loginData != null && myLogin.password != null)
                {
                    var Hasher = new PasswordHasher<Users>();
                    // Pass the user object, the hashed password, and the PasswordToCheck
                    if(0 != Hasher.VerifyHashedPassword(loginData, loginData.password, myLogin.password))
                    {
                        HttpContext.Session.SetInt32("UserID", loginData.id);
                        int? UserID = HttpContext.Session.GetInt32("UserID");
                        ViewBag.UserID = UserID;
                        return RedirectToAction("Wall");
                    }
                }
                return View("Index");
            }
            return View("Index");
        }
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
        
        [HttpPost("AddMessage")]
        public IActionResult AddMessage(Messages messager)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            var userInfo = _context.users.Where(p => p.id == UserID).SingleOrDefault();
            Messages myMessage = new Messages();
            myMessage.message = messager.message;
            myMessage.usersid = (int)UserID;
            myMessage.created_at = DateTime.Now;
            myMessage.updated_at = DateTime.Now;
            _context.Add(myMessage);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("MessageID", myMessage.id);
            int? MessageID = HttpContext.Session.GetInt32("MessageID");
            return RedirectToAction("Wall");
        }
        [HttpPost("AddComment")]
        public IActionResult AddComment(string comment, int messageid)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            var userInfo = _context.users.Where(p => p.id == UserID).SingleOrDefault();
            // int? MessageID = HttpContext.Session.GetInt32("MessageID");
            var messageInfo = _context.messages.Where(p => p.id == messageid).SingleOrDefault();
            Comments myComment = new Comments();
            myComment.comment = comment;
            myComment.usersid = (int)UserID;
            myComment.messagesid = messageid;
            myComment.created_at = DateTime.Now;
            myComment.updated_at = DateTime.Now;
            _context.Add(myComment);
            _context.SaveChanges();
            return RedirectToAction("Wall");
        }
        [HttpPost("DeleteMessage")]
        public IActionResult DeleteMessage(int messageid)
        {
            int? UserID = HttpContext.Session.GetInt32("UserID");
            var DelComment = _context.comments.Where(p => p.messagesid == messageid).ToList();
            var DelMessage = _context.messages.Where(p => p.id == messageid).SingleOrDefault();
            DateTime currentTime = DateTime.Now;
            TimeSpan creationTime = currentTime - DelMessage.created_at;

            if (creationTime.Minutes > 30)
            {
                foreach (var cmt in DelComment)
                {
                    _context.comments.Remove(cmt);
                }
                _context.messages.Remove(DelMessage);
            }
            else
            {
                TempData["message"] = "Message can only be deleted 30 minutes after creation";
            }
            _context.SaveChanges();
            return RedirectToAction("Wall");
        }
    }
}
