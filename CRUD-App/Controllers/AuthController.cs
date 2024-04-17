using CRUD_App.DbConfig;
using CRUD_App.Models;
using CRUD_App.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRUD_App.Controllers
{
    public class AuthController : Controller
    {
        AppDbContext _db;

        public AuthController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM loginVM)
        {
            UserModel user = _db.tbl_users.FirstOrDefault(options =>
                options.Email == loginVM.Email && options.Password == loginVM.Password
            );

            if (user != null)
            {
                if (loginVM.Email == user.Email && loginVM.Password == user.Password)
                {
                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.NameIdentifier, loginVM.Email));
                    claims.Add(new Claim(ClaimTypes.Name, loginVM.Email));

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    HttpContext.Session.SetString("uname", loginVM.Email);

                    HttpContext.SignInAsync(claimsPrincipal);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var storedCookies = Request.Cookies.Keys;

            foreach (var item in storedCookies)
            {
                Response.Cookies.Delete(item);
            }

            return RedirectToAction("Login");
        }
    }
}
