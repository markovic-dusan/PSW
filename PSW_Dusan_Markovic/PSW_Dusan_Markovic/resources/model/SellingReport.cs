using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PSW_Dusan_Markovic.resources.model
{
    public class SellingReport
    {
        [Key]
        public int ReportId { get; set; }
        [ForeignKey("Users")]
        public string AuthorId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int NoOfSoldTours {  get; set; }
        [Required]
        public decimal Profit {  get; set; }
        [Required]
        public decimal DeltaProfit { get; set; }
        [NotMapped]
        public List<Tour> BestSellers {  get; set; }
        [NotMapped]
        public List<Tour> NotSoldOnce {  get; set; }

        public SellingReport(string authorId, DateTime date, int soldTours, decimal profit, decimal deltaProfit, List<Tour> topSellers, List<Tour> notSold)
        {
            AuthorId = authorId;
            Date = date;
            NoOfSoldTours = soldTours;
            Profit = profit;
            DeltaProfit = deltaProfit;
            BestSellers = topSellers;
            NotSoldOnce = notSold;
        }

        public SellingReport(string authorId, DateTime date, int soldTours, decimal profit, decimal deltaProfit)
        {
            AuthorId = authorId;
            Date = date;
            NoOfSoldTours = soldTours;
            Profit = profit;
            DeltaProfit = deltaProfit;
            BestSellers = new List<Tour>();
            NotSoldOnce = new List<Tour>();
        }

        public SellingReport()
        {

        }
    }
}
