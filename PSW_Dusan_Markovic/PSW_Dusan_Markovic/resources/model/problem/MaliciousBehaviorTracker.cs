using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSW_Dusan_Markovic.resources.model.problem
{
    public class MaliciousBehaviorTracker
    {
        [Key]
        public int TrackerId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        public int NumberOfStrikes { get; set; }

        public MaliciousBehaviorTracker(string userId)
        {
            UserId = userId;
            NumberOfStrikes = 0;
        }

    }
}
