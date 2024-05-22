using Microsoft.AspNetCore.Mvc;

namespace PSW_Dusan_Markovic.resources.controllers
{
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("api/report")]
        public ActionResult<bool> generateReports()
        {
            return Ok(_reportService.generateReportForEachAuthor());
        }

        [HttpPost("api/users/{authorId}/report")]
        public ActionResult<SellingReport> generateAuthorReport(string authorId)
        {
            return Ok(_reportService.generateReport(authorId));
        }

        [HttpGet("api/users/{authorId}/report")]
        public ActionResult<List<SellingReport>> getAuthorReports(string authorId)
        {
            return Ok(_reportService.getReports(authorId));
        }

        [HttpGet("api/users/{authorId}/failingTours")]
        public ActionResult<List<Tour>> getFailingTours(string authorId)
        {
            return Ok(_reportService.getFailingTours(authorId));
        }
    }
}
