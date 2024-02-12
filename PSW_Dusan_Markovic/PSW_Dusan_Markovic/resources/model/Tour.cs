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

        [NotMapped]
        public List<Interest> Interests { get; set; }

        public bool IsDraft { get; set; }

        public bool IsPublished { get; set; }

        public List<KeyPoint> KeyPoints { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public Tour(string name, string description, EnumTourDifficulty difficulty, string category, decimal price, string authorId)
        {
            Name = name;
            Description = description;
            Difficulty = difficulty;
            Category = category;
            Price = price;
            IsDraft = true;
            IsPublished = false;
            KeyPoints = new List<KeyPoint>();
            Interests = new List<Interest>();
            AuthorId = authorId;
        }

        public Tour(string name, string description, EnumTourDifficulty difficulty, string category, decimal price, string authorId, List<Interest> interests)
        {
            Name = name;
            Description = description;
            Difficulty = difficulty;
            Category = category;
            Price = price;
            IsDraft = true;
            IsPublished = false;
            KeyPoints = new List<KeyPoint>();
            Interests = interests;
            AuthorId = authorId;
        }

        public void addKeyPoint(KeyPoint keyPoint)
        {
            keyPoint.TourId = TourId;
            KeyPoints.Add(keyPoint);
        }

        public void updateTour(Tour tour) { 
            Name = tour.Name;
            Description = tour.Description;
            Difficulty = tour.Difficulty;
            Category = tour.Category;
            Price = tour.Price;
            KeyPoints = tour.KeyPoints;
        }

        public void publishTour()
        {
            IsDraft = false;
            IsPublished = true;
        }

        public void archieveTour()
        {
            IsDraft = true;
            IsPublished = false;
        }
    }
}
