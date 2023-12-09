using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PSW_Dusan_Markovic.resources.model
{
    public class TourPurchase
    {
        [Key]
        public int Id{ get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Tour")]
        public int TourId { get; set; }

        [Required]
        public DateTime DateOfPurchase{ get; set; }
    }
}
