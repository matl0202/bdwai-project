using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrumkitStore.Models
{
    public class Zamowienie
    {
        public int Id { get; set; }
        public DateTime DataZamowienia { get; set; }

        [Required(ErrorMessage = "Brak ID użytkownika")] // kto zamowil
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "ID dla Drumkit jest wymagane")]//jaki drumkit zamowilismy
        public int DrumkitId { get; set; }

      
        public virtual Drumkit? Drumkit { get; set; }
    }
}