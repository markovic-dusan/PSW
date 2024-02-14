using System.Net.Mail;
using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PSW_Dusan_Markovic.resources.service
{
    public class MailService
    {
        private readonly YourDbContext _context; 

        public MailService(YourDbContext context)
        {
            _context = context;
        }

        public string getPurchaseHtml(List<Tour> pruchasedTours)
        {
            var html = "<h1> Thanks for your purchase!</h1>";
            html += "<p> You have confirmed the purchase of these tours:</p>";

            foreach (var tour in pruchasedTours)
            {
                html += $"<p> {tour.Name} </p>";
            }
            return html;
        }

        public async Task<bool> sendPurchaseEmail(string email, List<Tour> purchasedTours) {
            try
            {
                var apiKey = "your-sendgrid-api-key";
                var client = new SendGridClient(apiKey);

                var msg = new SendGridMessage
                {
                    From = new EmailAddress("tourshop@example.com", "Tour shop"),
                    Subject = "Tour purchase confirmation",
                    PlainTextContent = "Thanks for choosing our tours.",
                    HtmlContent = getPurchaseHtml(purchasedTours)
                };
                msg.AddTo(new EmailAddress(email));

                var response = await client.SendEmailAsync(msg);

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while sending email: {ex.Message}");
                return false;
            }
        }
    }
}
