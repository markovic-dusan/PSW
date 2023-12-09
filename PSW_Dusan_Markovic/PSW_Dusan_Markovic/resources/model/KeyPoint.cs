using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PSW_Dusan_Markovic.resources.model
{
    public class KeyPoint
    {
        [Key]
        public int PointId{ get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [ForeignKey("Tour")]
        public int TourId { get; set; }

        public Tour Tour { get; set; }
    }
}
