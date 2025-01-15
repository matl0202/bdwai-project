using Microsoft.EntityFrameworkCore;

namespace DrumkitStore.Models
{
    public class DrumkityDbContext : DbContext
    {
        public DrumkityDbContext(DbContextOptions<DrumkityDbContext> options):base(options) { }
        public DbSet<User> Users {get;set; }
        public DbSet<Drumkit> Drumkity {get;set; }
        public DbSet<Kategoria> Kategorie {get;set; }
        public DbSet<Zamowienie> Zamowienia {get;set; } // nazwa tabelki w sql to Zamowienia, a na podstawie klasy Zamowienie!!!
    }
}

