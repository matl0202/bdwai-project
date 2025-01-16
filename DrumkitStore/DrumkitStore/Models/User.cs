using System.ComponentModel.DataAnnotations;

namespace DrumkitStore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Podaj poprawny adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(100, ErrorMessage = "Hasło musi mieć od {2} do {1} znaków", MinimumLength = 6)]
        public string Password { get; set; }

        public int Role { get; set; } //1 =Admin 2 =User
        public virtual ICollection<Zamowienie> Zamowienia { get; set; } = new List<Zamowienie>();// jeden do wielu
    }
}