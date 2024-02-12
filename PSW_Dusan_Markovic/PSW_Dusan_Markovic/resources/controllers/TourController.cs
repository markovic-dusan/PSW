using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSW_Dusan_Markovic.resources.model;

namespace PSW_Dusan_Markovic.resources.controllers
{
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly TourService _tourService;

        public TourController(TourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet("api/tours")]
        [Authorize]
        public ActionResult<List<User>> getAllTours(string userId)
        {
            return Ok(_tourService.getAllTours);
        }

        [HttpPost("api/tours/{tourId}/purchase/{userId}")]
        [Authorize(Roles = "Tourist")]
        public ActionResult<bool> PurchaseTour(int tourId, string userId)
        {
            var result = _tourService.purchaseTour(tourId, userId);
            if (!result)
            {
                return BadRequest("Could not purchase tour.");
            }
            return Ok(result);
        }

        [HttpGet("api/tours/{id}")]
        [Authorize]
        public ActionResult<Tour> getTourById(int id)
        {
            var tour = _tourService.getTourById(id);
            if (tour == null)
            {
                return BadRequest("Tour not found.");
            }
            return Ok(tour);
        }

        [HttpGet("api/users/{userId}/mytours")]
        [Authorize]
        public ActionResult<List<User>> getUserTours(string userId)
        {
            var userTours = _tourService.getUserTours(userId);
            if (userTours == null)
            {
                return NotFound();
            }
            return Ok(userTours);
        }

        [HttpPost("api/users/{userId}/mytours")]
        [Authorize(Roles = "Author")]
        public ActionResult<bool> postTour(Tour tour)
        {
            var posted = _tourService.postTour(tour);
            if (!posted)
            {
                return BadRequest("Tour could not be posted.");
            }
            return Ok(posted);
        }

        [HttpPut("api/users/{userId}/mytours")]
        [Authorize(Roles = "Author")]
        public ActionResult<bool> updateTour(Tour tour)
        {
            var status = _tourService.updateTour(tour);
            if (!status)
            {
                return BadRequest("Tour could not be updated.");
            }
            return Ok(status);
        }

        [HttpPut("api/users/{userId}/mytours/{tourId}/publish")]
        [HttpPut("api/users/{userId}/mydrafttours/{tourId}/publish")]
        [Authorize(Roles = "Author")]
        public ActionResult<bool> publishTour(int tourId)
        {
            var status = _tourService.publishTour(tourId);
            if (!status)
            {
                return BadRequest("Tour could not be updated.");
            }
            return Ok(status);
        }

        [HttpPut("api/users/{userId}/mytours/{tourId}/archieve")]
        [HttpPut("api/users/{userId}/myactivetours/{tourId}/archieve")]
        [Authorize(Roles = "Author")]
        public ActionResult<bool> archieveTour(int tourId)
        {
            var status = _tourService.archieveTour(tourId);
            if (!status)
            {
                return BadRequest("Tour could not be updated.");
            }
            return Ok(status);
        }

        [HttpGet("api/users/{userId}/myactivetours")]
        [Authorize(Roles = "Author")]
        public ActionResult<List<User>> getUserActiveTours(string userId)
        {
            var userTours = _tourService.getUserActiveTour(userId);
            if (userTours == null)
            {
                return NotFound();
            }
            return Ok(userTours);
        }

        [HttpGet("api/users/{userId}/mydrafttours")]
        [Authorize(Roles = "Author")]
        public ActionResult<List<User>> getUserDraftTours(string userId)
        {
            var userTours = _tourService.getUserDraftTour(userId);
            if (userTours == null)
            {
                return NotFound();
            }
            return Ok(userTours);
        }

        [HttpGet("api/users/{userId}/recommended")]
        [Authorize(Roles = "Tourist")]
        public ActionResult<List<Tour>> getRecommendedTours(string userId)
        {
            var recommendedTours = _tourService.getRecommendedTours(userId);
            if (recommendedTours == null)
            {
                return NotFound();
            }
            return Ok(recommendedTours);
        }

    }
}
