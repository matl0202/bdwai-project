using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrumkitStore.Models
{
    public class Drumkit
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Nazwa { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string Opis { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cena { get; set; }




        public int KategoriaId { get; set; }

        public Kategoria Kategoria { get; set; }

    }
}
