using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSW_Dusan_Markovic.resources.model
{
    public class TourFailureMonitor
    {
        [Key]
        public int Id {  get; set; }
        [ForeignKey("Tour")]
        public int TourId { get; set; }
        [Required]
        public int TimesFailed {  get; set; }

        public TourFailureMonitor(int tourId, int timesFailed)
        {
            TourId = tourId;
            TimesFailed = timesFailed;
        }
    }
}
