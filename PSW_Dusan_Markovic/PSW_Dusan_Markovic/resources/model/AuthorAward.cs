using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSW_Dusan_Markovic.resources.model
{
    public class AuthorAward
    {
        [Key]
        public int AwardId { get; set; }    

        [ForeignKey("User")]
        public string AuthorId { get; set; }

        public int NumberOfAwards {  get; set; }

        public AuthorAward(string authorId, int numberOfAwards)
        {
            this.AuthorId = authorId; 
            this.NumberOfAwards = numberOfAwards;
        }

        public AuthorAward() { }
    }
}
