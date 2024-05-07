using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using PSW_Dusan_Markovic.resources.model;
using System.Linq;

namespace PSW_Dusan_Markovic.resources.service
{
    public class TourService
    {
        private readonly YourDbContext _context;

        public TourService(YourDbContext context)
        {
            _context = context;
        }

        //za autora vraca sve ture koje je kreirao, a za turistu sve koje je kupio
        public List<Tour> getUserTours(string userId)
        {
            User user = _context.Users.Find(userId);
            if (user == null)
            {
                return null;
            }

            if (user.UserType == UserType.AUTHOR)
            {   
                var createdTours =_context.Tours.Where(t => t.AuthorId == user.Id).ToList();
                getTourInterests(createdTours);
                return createdTours;
            } else if (user.UserType == UserType.TOURIST)
            {
                List<Tour> purchasedTours = new List<Tour>();
                List<TourPurchase> purchases = _context.TourPurchases.Where(t => t.UserId == user.Id).ToList();
                foreach (TourPurchase tp in purchases)
                {
                    purchasedTours.Add(_context.Tours.Find(tp.TourId));
                }
                getTourInterests(purchasedTours);
                return purchasedTours;
            }
            return null;
        }

        public List<Tour> getAllTours()
        {
            List<Tour> tours = _context.Tours.Where(t => t.IsPublished).ToList();
            getTourInterests(tours);
            return tours;
        }

        public List<Tour> getUserActiveTour(string userId)
        {
            User user = _context.Users.Find(userId);
            if (user == null)
            {
                return null;
            }
            List<Tour> tours = _context.Tours.Where(t => t.AuthorId == userId && t.IsPublished).ToList();
            getTourInterests(tours);
            return tours;
        }

        public List<Tour> getUserDraftTour(string userId)
        {
            User user = _context.Users.Find(userId);
            if (user == null)
            {
                Console.WriteLine("*****User not found");
                return null;
            }
            return _context.Tours.Where(t => t.AuthorId == userId && t.IsDraft).ToList();
        }

        public bool purchaseTour(TourPurchase tourPurchase)
        {
            Tour tour = _context.Tours.Find(tourPurchase.TourId);
            User user = _context.Users.Find(tourPurchase.UserId);
            if(tour == null || user == null || !tour.IsPublished)
            {
                return false;
            }
            _context.TourPurchases.Add(tourPurchase);
            _context.SaveChanges();
            return true;
        }

        public bool archieveTour(int tourId)
        {
            var tourToArchieve = _context.Tours.Find(tourId);
            if(tourToArchieve == null)
            {
                return false;
            }
            tourToArchieve.archieveTour();
            _context.SaveChanges();
            return true;
        }

        public bool publishTour(int tourId)
        {
            var tourToPublish = _context.Tours.Find(tourId);
            var keyPoints = _context.KeyPoints.Where(kp => kp.TourId == tourId).ToList();
            if (tourToPublish == null || keyPoints.Count < 2)
            {
                return false;
            }
            tourToPublish.publishTour();
            _context.SaveChanges();
            return true;
        }

        public bool updateTour(Tour tour, int tourId)
        {
            var tourToUpdate = _context.Tours.Find(tourId);
            if(tourToUpdate == null)
            {
                return false;
            }
            tourToUpdate.updateTour(tour);
            _context.SaveChanges();
            return true;
        }

        public List<Tour> getRecommendedTours(string userId)
        {
            var userInterests = _context.UserInterests
                .Where(ui => ui.UserId == userId)
                .Select(ui => ui.Interest)
                .ToList();

            foreach (var ui in userInterests)
            {
                Console.WriteLine($"User Interest: {ui}");
            }

            if (userInterests.Count == 0)
            {
                return null;
            }

            var matchingTours = (from ti in _context.TourInterests
                                 join t in _context.Tours on ti.TourId equals t.TourId
                                 where userInterests.Contains(ti.Interest) && t.IsPublished
                                 select t)
                                .Distinct()
                                .ToList();
            getTourInterests(matchingTours);
            return matchingTours;
        }

        public bool addKeypoint(KeyPoint kp)
        {
            _context.KeyPoints.Add(kp);
            _context.SaveChanges();
            return true;
        }

        public bool postTour(Tour tour)
        {
            if (tour.IsPublished)
            {
                return false;
            }
            _context.Tours.Add(tour);
            _context.SaveChanges();
            foreach (Interest i in tour.Interests)
            {
                _context.TourInterests.Add(new TourInterest(tour.TourId, i.InterestValue));
            }
            _context.SaveChanges();
            return true;
        }

        public Tour getTourById(int id)
        {
            return _context.Tours.Find(id);
        }

        public List<KeyPoint> getTourKeypoints(int tourId)
        {
            var tour = _context.Tours.Find(tourId);
            if (tour == null)
            {
                return null;
            }
            var keypoints = _context.KeyPoints.Where(kp => kp.TourId == tourId).ToList();
            return keypoints;
        }

        public void getTourInterests(List<Tour> tours)
        {
            foreach (var tour in tours)
            {
                List<TourInterest> tourInterests = _context.TourInterests.Where(ti => ti.TourId == tour.TourId).ToList();
                List<Interest> interests = new List<Interest>();
                foreach(var interest in tourInterests)
                {
                    interests.Add(new Interest(interest.Interest));
                }
                tour.Interests = interests;
            }

        }

    }
}
