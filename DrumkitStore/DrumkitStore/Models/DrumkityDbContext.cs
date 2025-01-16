using Microsoft.EntityFrameworkCore;

namespace DrumkitStore.Models
{
    public class DrumkityDbContext : DbContext
    {
        public DrumkityDbContext(DbContextOptions<DrumkityDbContext> options) : base(options) { }

        public DbSet<Drumkit> Drumkits { get; set; }
        public DbSet<Kategoria> Kategorie { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Relacja jeden-do-wielu: Drumkit -> Zamowienia
            modelBuilder.Entity<Zamowienie>()
                .HasOne(z => z.Drumkit) //zamowienie ma jeden Drumkit
                .WithMany() //drumkit może mieć wiele Zamowien
                .HasForeignKey(z => z.DrumkitId) //klucz obcy w Zamowienie
                .OnDelete(DeleteBehavior.Restrict); //zapobieganie usunięciu powiązanego Drumkit???

            base.OnModelCreating(modelBuilder);
        }
    }
}