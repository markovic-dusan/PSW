using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace PSW_Dusan_Markovic.resources.model
{
    public class Tour
    {
        [Key]
        public int TourId { get; set; }

        [Required]
        public string Name{ get; set; }

        public string Description{ get; set; }

        [Required]
        public EnumTourDifficulty Difficulty { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsDraft { get; set; }

        public bool IsPublished { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
    }
}
