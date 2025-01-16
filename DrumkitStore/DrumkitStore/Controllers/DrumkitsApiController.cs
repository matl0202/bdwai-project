using DrumkitStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrumkitStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace DrumkitStore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrumkitsApiController : ControllerBase
    {
        private readonly DrumkityDbContext _db;

        //Dodawanie DBContext przez konstruktor
        public DrumkitsApiController(DrumkityDbContext db)
        {
            _db = db;
        }

        // GET: api/DrumkitsApi
        [HttpGet]
        public ActionResult<IEnumerable<Drumkit>> GetDrumkits()
        {
            return _db.Drumkits.Include(d => d.Kategoria).ToList();
        }

        // GET: api/DrumkitsApi/5
        [HttpGet("{id}")]
        public ActionResult<Drumkit> GetDrumkit(int id)
        {
            var drumkit = _db.Drumkits.Include(d => d.Kategoria).FirstOrDefault(d => d.Id == id);
            if (drumkit == null)
            {
                return NotFound();
            }
            return drumkit;
        }

        // POST: api/DrumkitsApi
        [HttpPost]
        public ActionResult<Drumkit> CreateDrumkit(Drumkit model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Drumkits.Add(model);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetDrumkit), new { id = model.Id }, model);
        }

        // PUT: api/DrumkitsApi/5
        [HttpPut("{id}")]
        public IActionResult UpdateDrumkit(int id, Drumkit model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            _db.Entry(model).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.Drumkits.Any(d => d.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/DrumkitsApi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteDrumkit(int id)
        {
            var drumkit = _db.Drumkits.Find(id);
            if (drumkit == null)
            {
                return NotFound();
            }

            _db.Drumkits.Remove(drumkit);
            _db.SaveChanges();

            return NoContent();
        }
    }
}