﻿using Microsoft.AspNetCore.Identity;
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
        public List<Tour> getUserTours(string id)
        {
            User user = _context.Users.Find(id);
            if (user == null)
            {
                return null;
            }

            if (user.UserType == UserType.AUTHOR)
            {
                return _context.Tours.Where(t => t.AuthorId == user.Id).ToList();
            } else if (user.UserType == UserType.TOURIST)
            {
                List<Tour> purchasedTours = new List<Tour>();
                List<TourPurchase> purchases = _context.TourPurchases.Where(t => t.UserId == user.Id).ToList();
                foreach (TourPurchase tp in purchases)
                {
                    purchasedTours.Add(_context.Tours.Find(tp.TourId));
                }
                return purchasedTours;
            }
            return null;
        }

        public List<Tour> getAllTours()
        {
            return _context.Tours.Where(t => t.IsPublished).ToList();
        }

        public List<Tour> getUserActiveTour(string userId)
        {
            User user = _context.Users.Find(userId);
            if (user == null)
            {
                return null;
            }
            return _context.Tours.Where(t => t.AuthorId == userId && t.IsPublished).ToList();
        }

        public List<Tour> getUserDraftTour(string userId)
        {
            User user = _context.Users.Find(userId);
            if (user == null)
            {
                return null;
            }
            return _context.Tours.Where(t => t.AuthorId == userId && t.IsDraft).ToList();
        }

        public bool purchaseTour(int tourId, string userId)
        {
            Tour tour = _context.Tours.Find(tourId);
            User user = _context.Users.Find(userId);
            if(tour == null || user == null || !tour.IsPublished)
            {
                return false;
            }
            TourPurchase purchase = new TourPurchase(userId, tourId, DateTime.Today);
            _context.TourPurchases.Add(purchase);
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
            if (tourToPublish == null || tourToPublish.KeyPoints.Count < 2)
            {
                return false;
            }
            tourToPublish.publishTour();
            _context.SaveChanges();
            return true;
        }

        public bool updateTour(Tour tour)
        {
            var tourToUpdate = _context.Tours.Find(tour.TourId);
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

            return matchingTours;
        }

        public bool postTour(Tour tour)
        {
            foreach(Interest i in tour.Interests)
            {
                _context.TourInterests.Add(new TourInterest(tour.TourId, i.InterestValue));
                _context.SaveChanges();
            }
            if (tour.IsPublished)
            {
                return false;
            }
            _context.Tours.Add(tour);
            return true;
        }

    }
}
