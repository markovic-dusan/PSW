using Microsoft.AspNetCore.Mvc;

namespace PSW_Dusan_Markovic.resources.controllers
{
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;
        private readonly LoginService _loginService;

        public ReportController(ReportService reportService, LoginService loginService)
        {
            _reportService = reportService;
            _loginService = loginService;
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
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.AUTHOR))
            {
                return Ok(_reportService.getReports(authorId));
            }
            return Unauthorized();
        }

        [HttpGet("api/users/{authorId}/failingTours")]
        public ActionResult<List<Tour>> getFailingTours(string authorId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.AUTHOR))
            {
                return Ok(_reportService.getFailingTours(authorId));
            }
            return Unauthorized();
        }
    }
}
