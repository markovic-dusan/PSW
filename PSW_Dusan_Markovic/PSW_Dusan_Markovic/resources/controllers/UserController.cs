using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PSW_Dusan_Markovic.resources.model;
using PSW_Dusan_Markovic.resources.service;
using System.Security.Claims;

namespace PSW_Dusan_Markovic.resources.controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly TourService _tourService;
        private readonly MailService _mailService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService service, TourService tourService) //, MailService mailService
        {
            _service = service;
            _tourService = tourService;
            //_mailService = mailService;
        }

        [HttpGet]
        public ActionResult<List<User>> getUsers()
        {
            return Ok(_service.getAllUsers());            
        }

        [HttpGet("{id}")]
        //[Authorize]
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
        //[Authorize]
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
        //[Authorize]
        public ActionResult<bool> deleteUser(string id)
        {
            var status = _service.deleteUser(id);
            if (!status)
            {
                return BadRequest("User could not be deleted.");
            }
            return Ok(status);
        }

        [HttpPost("{userId}/purchase")]
        //[Authorize(Roles = "TOURIST")]
        public ActionResult<bool> PurchaseTour(List<Tour> purchasedTours, string userId)
        {
            var result = true;
            foreach(var tour in purchasedTours)
            {
                result = _tourService.purchaseTour(tour.TourId, userId);
                if (!result)
                {
                    return BadRequest("Could not purchase tours.");
                }
            }
            var user = _service.getUserById(userId);
            //var mailSent = _mailService.sendPurchaseEmail(user.Email, purchasedTours);
            //if (!mailSent.Result)
            //{
            //    return BadRequest("Could not confirm purchase.");
            //}
            return Ok();
        }
    }

}
