using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace PSW_Dusan_Markovic.resources.model
{
    public class UserInterest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Interest")]
        public EnumInterest Interest { get; set; }

        public UserInterest(string userId, EnumInterest interest)
        {
            UserId = userId;
            Interest = interest;
        }
    }
}
