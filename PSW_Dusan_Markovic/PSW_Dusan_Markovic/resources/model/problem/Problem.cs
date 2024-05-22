using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSW_Dusan_Markovic.resources.model.problem
{
    public class Problem
    {
        [Key]
        public int ProblemId { get; set; }
        [ForeignKey("User")]
        public string TouristId {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public int TourId { get; set; }
        public event EventHandler<ProblemStatusChangedEvent> StatusChanged;
        [NotMapped]
        public EnumProblemStatus Status { get; set; }

        public Problem() { }
        public Problem(string touristId, string title, string description, int tourId)
        {
            TouristId = touristId;
            Title = title;
            Description = description;
            TourId = tourId;
        }
        public void changeStatus(EnumProblemStatus status)
        {
            Status = status;
            onStatusChanged(new ProblemStatusChangedEvent(ProblemId, status));
        }
        protected void onStatusChanged(ProblemStatusChangedEvent e)
        {
            StatusChanged?.Invoke(this, e);
        }
    }

    
}
