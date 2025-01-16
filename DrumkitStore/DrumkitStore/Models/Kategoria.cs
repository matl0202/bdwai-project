using DrumkitStore.Models;
using System.ComponentModel.DataAnnotations;

namespace DrumkitStore.Models
{
    public class Kategoria
    {
        public int Id { get; set; }
        [Required]
        public string Nazwa { get; set; }
        public virtual ICollection<Drumkit> Drumkits { get; set; }
    }

}
