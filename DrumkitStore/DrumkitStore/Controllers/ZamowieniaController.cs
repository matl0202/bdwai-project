using DrumkitStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DrumkitStore.Controllers
{
    public class ZamowieniaController : Controller
    {
        private readonly DrumkityDbContext _db;

        public ZamowieniaController(DrumkityDbContext db)
        {
            _db = db;
        }

        // GET: /Zamowienia/Create
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Drumkits= _db.Drumkits.ToList(); //pobranie listy drumkitów do widoku
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Zamowienie model)
        {

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;            //pobieranie e-maila zalogowanego uzyt

            if (string.IsNullOrEmpty(userEmail))
            {
                ModelState.AddModelError("", "Nie można pobrac adresu e-mail");
                ViewBag.Drumkits= _db.Drumkits.ToList();
                return View(model);
            }


            var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);    //znajdź użytkownika na podstawie e-maila
            if (user== null)
            {
                ModelState.AddModelError("", "Nie znaleziono uzytkownika z tym adresem e-mail");
                ViewBag.Drumkits = _db.Drumkits.ToList();
                return View(model);
            }

            model.UserId= user.Id;
            model.DataZamowienia = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.Zamowienia.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index", "Zamowienia");
            }

            ViewBag.Drumkits = _db.Drumkits.ToList();
            return View(model);
        }

        [Authorize]
        public ActionResult MojeZamowienia()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value; 
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            //uzytkownik na podstawie emaila
            var user= _db.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //pobranie zamowienia
            var zamowienia = _db.Zamowienia
                .Where(z => z.UserId == user.Id)
                .Include(z => z.Drumkit)
                .ToList();

            return View(zamowienia);
        }

        
        [Authorize(Roles = "Admin")]
        public IActionResult Manage()
        {
            var zamowienia = _db.Zamowienia.Include(z=> z.Drumkit).Include(z => z.User).ToList();
            return View(zamowienia);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var zamowienie = _db.Zamowienia.Find(id);
            if (zamowienie != null)
            {
                _db.Zamowienia.Remove(zamowienie);
                _db.SaveChanges();
            }
            return RedirectToAction("Manage");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var zamowienie = _db.Zamowienia.Include(z => z.Drumkit).FirstOrDefault(z => z.Id == id);
            if (zamowienie == null)
            {
                return NotFound();
            }

            ViewBag.Drumkits = _db.Drumkits.ToList();
            return View(zamowienie);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Zamowienie model)
        {
            if (ModelState.IsValid)
            {
                _db.Zamowienia.Update(model);
                _db.SaveChanges();
                return RedirectToAction("Manage");
            }

            ViewBag.Drumkits = _db.Drumkits.ToList();
            return View(model);
        }
    }
}
