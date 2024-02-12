using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PSW_Dusan_Markovic.resources.model;
using PSW_Dusan_Markovic.resources.service;

namespace PSW_Dusan_Markovic.resources.controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly UserManager<User> _userManager;


        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<User>> getUsers()
        {
            return Ok(_service.getAllUsers());
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<User> getUserById(string id)
        {
            var user = _service.getUserById(id);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            return Ok(user);
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
        [Authorize]
        public ActionResult<bool> updateUser(User user)
        {
            var status = _service.updateUser(user);
            if(!status)
            {
                return BadRequest("User could not be updated.");
            }
            return Ok(status);
        }

        [HttpDelete("{id}")]
        [Authorize]
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
