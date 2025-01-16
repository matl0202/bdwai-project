using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrumkitStore.Models
{
    public class Drumkit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        public string Nazwa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cena jest wymagana.")]
        [Range(0.01, 10000, ErrorMessage = "Cena musi być pomiędzy 0.01 a 10 000.")]
        [Column(TypeName = "decimal(18,2)")] 
        public decimal Cena { get; set; }

        [Required(ErrorMessage = "Kategoria jest wymagana.")]
        public int KategoriaId { get; set; }

        public virtual Kategoria? Kategoria { get; set; }
    }
}