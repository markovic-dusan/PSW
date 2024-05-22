using System.ComponentModel.DataAnnotations;

namespace PSW_Dusan_Markovic.resources.model.problem
{
    public class ProblemStatusChangedEvent
    {
        [Key]
        public int Id { get; set; }
        public int ProblemId { get; set; }
        public EnumProblemStatus NewStatus { get; set; }
        public DateTime Timestamp { get; set; }
        public ProblemStatusChangedEvent() { }
        public ProblemStatusChangedEvent(int problemId, EnumProblemStatus newStatus)
        {
            ProblemId = problemId;
            NewStatus = newStatus;
            Timestamp = DateTime.Now;
        }   
    }
}
