using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace PSW_Dusan_Markovic.resources.controller
{
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly AdministrationService _administrationService;
        private readonly LoginService _loginService;

        public AdministratorController(AdministrationService administrationService, LoginService loginService)
        {
            _administrationService = administrationService;
            _loginService = loginService;
        }

        [HttpPut("api/admin/{userId}/block")]
        public ActionResult<bool> blockUser(string userId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.ADMIN))
            {
                var ret = _administrationService.blockUser(userId);
                if (ret)
                {
                    return Ok("User blocked");
                }
                return BadRequest("User not found");
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPut("api/admin/{userId}/unblock")]
        public ActionResult<bool> unblockUser(string userId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.ADMIN))
            {
                var ret = _administrationService.unblockUser(userId);
                if (ret)
                {
                    return Ok("User unblocked");
                }
                return BadRequest("User not found");
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("api/admin/malicious")]
        //[Authorize]
        public ActionResult<List<User>> getMaliciousUsers()
        {
            var token = HttpContext.Request.Headers["Authorization"];            
            Console.WriteLine($"Received token: {token}");

            if (_loginService.authorize(token, UserType.ADMIN))
            {
                return Ok(_administrationService.getMaliciousUsers());
            }
            else
            {
                return Unauthorized(token);
            }
        }

        [HttpGet("api/admin/blocked")]
        public ActionResult<List<User>> getBlockedUsers()
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.ADMIN))
            {
                return Ok(_administrationService.getBlockedUsers());
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
