using Microsoft.AspNetCore.Mvc;

namespace PSW_Dusan_Markovic.resources.controller
{
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly AdministrationService _administrationService;
        
        public AdministratorController(AdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        [HttpPut("api/admin/{userId}/block")]
        public ActionResult<bool> blockUser(string userId)
        {
            var ret = _administrationService.blockUser(userId);
            if (ret)
            {
                return Ok("User blocked");
            }
            return BadRequest("User not found");
        }

        [HttpPut("api/admin/{userId}/unblock")]
        public ActionResult<bool> unblockUser(string userId)
        {
            var ret = _administrationService.unblockUser(userId);
            if (ret)
            {
                return Ok("User unblocked");
            }
            return BadRequest("User not found");
        }

        [HttpGet("api/admin/malicious")]
        public ActionResult<List<User>> getMaliciousUsers()
        {
            return Ok(_administrationService.getMaliciousUsers());
        }

        [HttpGet("api/admin/blocked")]
        public ActionResult<List<User>> getBlockedUsers()
        {
            return Ok(_administrationService.getBlockedUsers());
        }
    }
}
