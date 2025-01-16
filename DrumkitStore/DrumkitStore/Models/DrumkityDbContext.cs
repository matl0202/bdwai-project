using Microsoft.EntityFrameworkCore;

namespace DrumkitStore.Models
{
    public class DrumkityDbContext : DbContext
    {
        public DrumkityDbContext(DbContextOptions<DrumkityDbContext> options) : base(options) { }

        public DbSet<Drumkit> Drumkits { get; set; }
        public DbSet<Kategoria> Kategorie { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Relacja 1:n: drumkit a zamowienia
            modelBuilder.Entity<Zamowienie>()
                .HasOne(z => z.Drumkit) 
                .WithMany() 
                .HasForeignKey(z => z.DrumkitId) 
                .OnDelete(DeleteBehavior.Restrict); 

            //1:n user a zamowienie
            modelBuilder.Entity<Zamowienie>()
                .HasOne(z => z.User) 
                .WithMany(u => u.Zamowienia) 
                .HasForeignKey(z => z.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id= 1,
                    Email = "admin@email.com",
                    Password = "admin123", 
                    Role= 1 
                },
                new User
                {
                    Id= 2,
                    Email = "user@email.com",
                    Password = "user123",
                    Role= 2 
                }
            );

            modelBuilder.Entity<Kategoria>().HasData( //dodawanie kategorii na start
                new Kategoria
                {
                    Id= 1,
                    Nazwa = "Drumkit"
                },
                new Kategoria
                {
                    Id= 2,
                    Nazwa ="Sample Pack"
                },
                new Kategoria
                {
                    Id= 3,
                    Nazwa ="Oneshot Pack"
                }
            );


        }



    }
}