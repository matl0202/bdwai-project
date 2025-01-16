using DrumkitStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DrumkitStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly DrumkityDbContext _db;

        public AccountController(DrumkityDbContext context) 
        {
            _db= context;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View() ;
        }


        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                if (_db.Users.Any(u=> u.Email== model.Email))
                {
                    ModelState.AddModelError("Email","Ten e-mail jest już zajęty!");
                    return View(model);
                }

                model.Role= 2;// ze domuyslnie zwykyl user

                _db.Users.Add(model);
                _db.SaveChanges();

                return RedirectToAction("Login","Account");
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult Login(string returnUrl= null)//zeby ta sama strona byla jak juz sie zalogujemy
        {
            ViewBag.ReturnUrl= returnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            var user= _db.Users.FirstOrDefault(u=> u.Email== email && u.Password == password);

            if (user==null)
            {
                ViewBag.ErrorMessage= "Nieprawidłowy e-mail lub hasło";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role== 1 ? "Admin" : "User")
    };

            var claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Drumkits");
        }

        [Authorize]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync();

            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))//przekierowanie na strone na kotrej sie wylogowywalismy
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Drumkits");
        }



        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}