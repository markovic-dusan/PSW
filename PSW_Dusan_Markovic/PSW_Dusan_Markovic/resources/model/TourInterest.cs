using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Query;

namespace PSW_Dusan_Markovic.resources.model
{
    public class TourInterest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tour")]
        public int TourId { get; set; }

        [ForeignKey("Interest")]
        public EnumInterest Interest { get; set; }

        public TourInterest(int tourId, EnumInterest interest)
        {
            TourId = tourId;
            Interest = interest;
        }
    }
}
