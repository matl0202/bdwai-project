using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrumkitStore.Models
{
    public class Zamowienie
    {
        [Key]
        public int Id { get; set; }




        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }




        public int DrumkitId { get; set; }

        [ForeignKey("DrumkitId")]
        public Drumkit Drumkit { get; set; }




        [Column(TypeName = "datetime")]
        public DateTime ZamowienieDate { get; set; } = DateTime.Now;
    }
}
