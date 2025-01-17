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
        public IActionResult ListaZamowien()
        {
            var zamowienia = _db.Zamowienia
                .Include(z => z.Drumkit)
                .Include(z => z.User)
                .OrderByDescending(z => z.DataZamowienia)
                .ToList();

            return View(zamowienia);
        }



        [HttpPost]
        [Authorize]
        public IActionResult ZlozZamowienie(int drumkitId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var drumkit = _db.Drumkits.FirstOrDefault(d => d.Id == drumkitId);
            if (drumkit == null)
            {
                return NotFound("Nie znaleziono danego drumkita...");
            }

            var zamowienie = new Zamowienie
            {
                UserId = user.Id,
                DrumkitId = drumkitId,
                DataZamowienia = DateTime.Now
            };

            Console.WriteLine($"Dodanie zamówienia UserId={zamowienie.UserId}, DrumkitId={zamowienie.DrumkitId}, DataZamowienia={zamowienie.DataZamowienia}"); //errory znow

            if (ModelState.IsValid)
            {
                _db.Zamowienia.Add(zamowienie);
                _db.SaveChanges();

                TempData["SuccessMessage"] = "Twoje zamówienie zostało zatwierdzone!";
                return RedirectToAction("MojeZamowienia", "Zamowienia");
            }

            return RedirectToAction("Index", "Drumkits");
        }

        [Authorize]
        public IActionResult PotwierdzZamowienie(int drumkitId)
        {
            var drumkit = _db.Drumkits.Include(d => d.Kategoria).FirstOrDefault(d => d.Id == drumkitId);

            if (drumkit == null)
            {
                return NotFound("Nie znaleziono drumkita");
            }

            var model = new Zamowienie
            {
                DrumkitId = drumkit.Id,
                Drumkit = drumkit,
                DataZamowienia = DateTime.Now
            };

            return View(model); 
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
