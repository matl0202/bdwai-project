using DrumkitStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrumkitStore.Controllers
{
    [Authorize]
    public class DrumkitsController : Controller
    {
        private readonly DrumkityDbContext _db;

        public DrumkitsController(DrumkityDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var drumkits = _db.Drumkits.Include(d => d.Kategoria).ToList();
            return View(drumkits);
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Kategorie = _db.Kategorie.ToList();
            Console.WriteLine($"Kategorie: {string.Join(", ", _db.Kategorie.Select(k => k.Nazwa))}");            //logowanie danych kategorii

            return View();
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Drumkit model)
        {
            //do errorow znow
            Console.WriteLine($"Dane przesłane do kontrolera:");
            Console.WriteLine($"Nazwa: {model.Nazwa}");
            Console.WriteLine($"Cena: {model.Cena}");
            Console.WriteLine($"KategoriaId: {model.KategoriaId}");

            if (!ModelState.IsValid)            //Model state do errorowania
            {
                Console.WriteLine("ModelState nieprawidlowy. Błąd valid:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Błąd valid: {error.ErrorMessage}");
                }

                ViewBag.Kategorie = _db.Kategorie.ToList();
                return View(model);
            }

            _db.Drumkits.Add(model);
            _db.SaveChanges();

            Console.WriteLine("Drumkit dodany pomyślnie");
            return RedirectToAction("Index");
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            
            var drumkit = _db.Drumkits.Find(id);//pobieranie drumkita na podstawie id
            if (drumkit == null)
            {
                return NotFound(); //jesli nie znajdzie to blad
            }

            ViewBag.Kategorie = _db.Kategorie.ToList();//pobieranie listy kategori i wrzucanie do viewbaga

            return View(drumkit); //przekazanie drumkitu do naszego widoku
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Drumkit model)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var drumkit = _db.Drumkits.Find(id);
            if (drumkit != null)
            {
                var powiazaneZamowienia = _db.Zamowienia.Any(z => z.DrumkitId == id);

                if (powiazaneZamowienia)
                {
                    //verify
                    TempData["ErrorMessage"] = "Nie można usunac wybranej Paczki Dźwięków - istnieja powiązania w DB";
                    return RedirectToAction("Index");
                }

                _db.Drumkits.Remove(drumkit);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Kup(int id)
        {
            var drumkit = _db.Drumkits.Include(d => d.Kategoria).FirstOrDefault(d => d.Id == id);

            if (drumkit == null)
            {
                return NotFound("Nie znaleziono wybranego drumkita");
            }

            var model = new Zamowienie
            {
                DrumkitId = drumkit.Id,
                Drumkit = drumkit,
                DataZamowienia = DateTime.Now
            };

            return RedirectToAction("PotwierdzZamowienie", "Zamowienia", new { drumkitId = drumkit.Id });
        }
    }
}


