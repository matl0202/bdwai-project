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

        //jaki drumkit zamowilismy
        [Required(ErrorMessage = "Drumkit ID jest wymagane")]
        public int DrumkitId { get; set; }

      
        public virtual Drumkit? Drumkit { get; set; }
    }
}