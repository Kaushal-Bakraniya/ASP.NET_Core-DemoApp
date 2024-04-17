using CRUD_App.DbConfig;
using CRUD_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        AppDbContext _db;

        IHttpContextAccessor _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _db = db;
            _context = httpContextAccessor;
        }


        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            /*CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            };

            _context.HttpContext.Response.Cookies.Append("Uname", "TEST",options);

            _context.HttpContext.Session.SetString("MyFullName","TEST");*/

            IEnumerable<UserModel> userModels = _db.tbl_users;
            return View(userModels);
        }

        [HttpGet]
        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Insert(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                _db.tbl_users.Add(userModel);
                _db.SaveChanges();

                TempData["msg"] = "Record Inserted Successfully.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", ModelState.Values.ToString());
            return View(userModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            UserModel obj = _db.tbl_users.Find(id);
            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                _db.tbl_users.Update(userModel);
                _db.SaveChanges();

                TempData["msg"] = "Record Updated Successfully.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", ModelState.Values.ToString());
            return View(userModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            UserModel obj = _db.tbl_users.Find(id);

            _db.tbl_users.Remove(obj);
            _db.SaveChanges();

            TempData["msg"] = "Record Deleted Successfully.";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
