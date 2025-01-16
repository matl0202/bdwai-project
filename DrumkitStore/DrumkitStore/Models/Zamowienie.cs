using System.ComponentModel.DataAnnotations;

namespace DrumkitStore.Models
{
    public class Zamowienie
    {
        public int Id { get; set; }
        public DateTime DataZamowienia { get; set; }

        [Required(ErrorMessage = "Identyfikator użytkownika jest wymagany.")]
        public string UserId { get; set; } = string.Empty;

        //jaki drumkit zamowilismy
        [Required(ErrorMessage = "Drumkit jest wymagany.")]
        public int DrumkitId { get; set; }

      
        public virtual Drumkit? Drumkit { get; set; }
    }
}