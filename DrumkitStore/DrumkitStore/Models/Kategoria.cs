using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrumkitStore.Models
{
    public class Kategoria
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Nazwa { get; set; }




        public List<Drumkit> Drumkits { get; set; }
    }
}
