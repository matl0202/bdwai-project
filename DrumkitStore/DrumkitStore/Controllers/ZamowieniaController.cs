using DrumkitStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrumkitStore.Controllers
{
    public class ZamowieniaController : Controller
    {
        private readonly DrumkityDbContext _db;

        //Konstruktor z dodaniem zależności
        public ZamowieniaController(DrumkityDbContext db)
        {
            _db = db;
        }

        // GET: /Zamowienia/Create
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Drumkits = _db.Drumkits.ToList(); //Pobranie listy drumkitów do widoku
            return View(); 
        }

        // POST: /Zamowienia/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Zamowienie model)
        {
            if (!ModelState.IsValid)
            {
                //jesli bład walidacyjny to znow pokazuje formularz
                ViewBag.Drumkits = _db.Drumkits.ToList();
                return View(model);
            }

            //przypisanie danych do zamowienia
            model.DataZamowienia = DateTime.Now;
            model.UserId = User.Identity.Name;

            //dodanie naszego zamówienia do bazy danych
            _db.Zamowienia.Add(model);
            _db.SaveChanges();

            return RedirectToAction("MojeZamowienia");
        }

        [Authorize]
        public ActionResult MojeZamowienia()
        {
            var userId = User.Identity.Name;
            var zamowienia = _db.Zamowienia
                .Where(z => z.UserId == userId)
                .Include(z => z.Drumkit) //wczytanie drumkitów które sa powiazane 
                .ToList();
            return View(zamowienia);
        }
    }
}