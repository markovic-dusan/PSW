using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSW_Dusan_Markovic.resources.model;

namespace PSW_Dusan_Markovic.resources.controllers
{
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly TourService _tourService;
        private readonly LoginService _loginService;

        public TourController(TourService tourService, LoginService loginService)
        {
            _tourService = tourService;
            _loginService = loginService;
        }

        [HttpGet("api/tours")]
        public ActionResult<List<Tour>> getAllTours()
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST, UserType.AUTHOR))
            {
                return Ok(_tourService.getAllTours());
            }
            return Unauthorized();
        }

        [HttpGet("api/tours/awarded")]
        public ActionResult<List<Tour>> getRewardedTours()
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST, UserType.AUTHOR))
            {
                return Ok(_tourService.getRewardedTours());
            }
            return Unauthorized();
        }

        [HttpGet("api/tours/{id}/keypoints")]
        public ActionResult<KeyPoint> getTourKeypoints(int id)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST, UserType.AUTHOR))
            {
                var keypoints = _tourService.getTourKeypoints(id);
                if (keypoints == null)
                {
                    return BadRequest("Tour not found.");
                }
                return Ok(keypoints);
            }
            return Unauthorized();
        }

        [HttpGet("api/tours/{id}")]
        public ActionResult<Tour> getTourById(int id)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST, UserType.AUTHOR))
            {
                var tour = _tourService.getTourById(id);
                if (tour == null)
                {
                    return BadRequest("Tour not found.");
                }
                return Ok(tour);
            }
            return Unauthorized();
        }

        [HttpGet("api/users/{userId}/mytours")]
        public ActionResult<List<User>> getUserTours(string userId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST, UserType.AUTHOR))
            {
                var userTours = _tourService.getUserTours(userId);
                if (userTours == null)
                {
                    return NotFound();
                }
                return Ok(userTours);
            }
            return Unauthorized();
        }

        [HttpPost("api/tours/{tourId}/keypoints")]
        public ActionResult<bool> addKeypoint(KeyPoint kp)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.AUTHOR))
            {
                var added = _tourService.addKeypoint(kp);
                if (!added)
                {
                    return BadRequest("Keypoint could not be added.");
                }
                return Ok(added);
            }
            return Unauthorized();
        }

        [HttpPost("api/users/{userId}/mytours")]
        public ActionResult<bool> postTour(Tour tour)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.AUTHOR))
            {
                var posted = _tourService.postTour(tour);
                if (!posted)
                {
                    return BadRequest("Tour could not be posted.");
                }
                return Ok(posted);
            }
            return Unauthorized();
        }

        [HttpPut("api/users/{userId}/mytours/{tourId}")]
        public ActionResult<bool> updateTour(Tour tour, int tourId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.AUTHOR))
            {
                var status = _tourService.updateTour(tour, tourId);
                if (!status)
                {
                    return BadRequest("Tour could not be updated.");
                }
                return Ok(status);
            }
            return Unauthorized() ;
        }

        [HttpPut("api/users/{userId}/mytours/{tourId}/publish")]
        [HttpPut("api/users/{userId}/mydrafttours/{tourId}/publish")]
        public ActionResult<bool> publishTour(int tourId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.AUTHOR))
            {
                var status = _tourService.publishTour(tourId);
                if (!status)
                {
                    return BadRequest("Tour could not be updated.");
                }
                return Ok(status);
            }
            return Unauthorized();
        }

        [HttpPut("api/users/{userId}/mytours/{tourId}/archieve")]
        [HttpPut("api/users/{userId}/myactivetours/{tourId}/archieve")]
        public ActionResult<bool> archieveTour(int tourId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.AUTHOR))
            {
                var status = _tourService.archieveTour(tourId);
                if (!status)
                {
                    return BadRequest("Tour could not be updated.");
                }
                return Ok(status);
            }
            return Unauthorized();
        }

        [HttpGet("api/users/{userId}/myactivetours")]
        public ActionResult<List<User>> getUserActiveTours(string userId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.AUTHOR))
            {
                var userTours = _tourService.getUserActiveTour(userId);
                if (userTours == null)
                {
                    return NotFound();
                }
                return Ok(userTours);
            }
            return Unauthorized();
        }

        [HttpGet("api/users/{userId}/mydrafttours")]
        public ActionResult<List<Tour>> getUserDraftTours(string userId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.AUTHOR))
            {
                Console.WriteLine("*****Controller");
                var userTours = _tourService.getUserDraftTour(userId);
                if (userTours == null)
                {
                    return BadRequest();
                }
                return Ok(userTours);
            }
            return Unauthorized();
        }

        [HttpGet("api/users/{userId}/recommended")]
        public ActionResult<List<Tour>> getRecommendedTours(string userId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST))
            {
                var recommendedTours = _tourService.getRecommendedTours(userId);
                if (recommendedTours == null)
                {
                    return NotFound();
                }
                return Ok(recommendedTours);
            }
            return Unauthorized() ;
        }

        [HttpPost("api/tours/{tourId}")]
        public ActionResult<Tour> purchaseTour(TourPurchase tourPurchase)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST))
            {
                var purchased = _tourService.purchaseTour(tourPurchase);
                if (!purchased)
                {
                    return BadRequest("Tour could not be purchased.");
                }
                return Ok(purchased);
            }
            return Unauthorized();
        }
    }
}
