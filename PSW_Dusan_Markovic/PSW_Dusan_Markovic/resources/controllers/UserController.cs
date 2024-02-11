using Microsoft.AspNetCore.Http;
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

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<User>> getUsers()
        {
            return Ok(_service.getAllUsers());
        }

        [HttpGet("{id}")]
        public ActionResult<User> getUserById(int id)
        {
            var user = _service.getUserById(id);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<bool> registerUser(User user)
        {
            var registered = _service.registerUser(user);
            if (!registered)
            {
                return BadRequest("User could not be registered");
            }
            return Ok(registered);
        }

        [HttpPut("{id}")]
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
        public ActionResult<bool> deleteUser(int id)
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
