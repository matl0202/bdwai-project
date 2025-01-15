using DrumkitStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DrumkitStore.Controllers
{
    public class DrumkitController : Controller
    {

        private readonly DrumkityDbContext _context;
        public DrumkitController(DrumkityDbContext context)
        {
            _context = context;
        }

        // GET: DrumkitController
        public ActionResult Index()
        {

            var drumkits = _context.Drumkity.Include(d => d.Kategoria).ToList();
            return View(drumkits);
        }

        // GET: DrumkitController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DrumkitController/Create
        public ActionResult Create()
        {
            ViewBag.Kategorie = new SelectList(_context.Kategorie, "Id", "Nazwa");
            return View();
        }

        // POST: DrumkitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Drumkit drumkit)
        {
            try
            {
                _context.Drumkity.Add(drumkit);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DrumkitController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DrumkitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DrumkitController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DrumkitController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
