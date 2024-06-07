using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PSW_Dusan_Markovic.resources.model;
using PSW_Dusan_Markovic.resources.service;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace PSW_Dusan_Markovic.resources.controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly TourService _tourService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly LoginService _loginService;

        public UserController(UserService service, TourService tourService, LoginService loginService)
        {
            _service = service;
            _tourService = tourService;
            _loginService = loginService;
        }

        [HttpGet]
        public ActionResult<List<User>> getUsers()
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.ADMIN))
            {
                return Ok(_service.getAllUsers());
            }
            return Unauthorized();
        }

        [HttpGet("{id}")]
        public ActionResult<User> getUserById(string id)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST, UserType.AUTHOR))
            {
                var user = _service.getUserById(id);
                if (user == null)
                {
                    return BadRequest("User not found.");
                }
                return Ok(user);
            }
            return Unauthorized();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> registerUser(User user)
        {
            var registered = await _service.registerUser(user);            
            if (!registered)
            {
                return BadRequest("User could not be registered");
            }
            return Ok(registered);
        }

        [HttpPut("{id}")]
        public ActionResult<bool> updateUser(User user)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST, UserType.AUTHOR))
            {
                var status = _service.updateUser(user);
                if (!status)
                {
                    return BadRequest("User could not be updated.");
                }
                return Ok(status);
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> deleteUser(string id)
        {
            var status = _service.deleteUser(id);
            if (!status)
            {
                return BadRequest("User could not be deleted.");
            }
            return Ok(status);
        }
    }

}
