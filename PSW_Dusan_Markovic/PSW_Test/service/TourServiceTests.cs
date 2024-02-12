using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PSW_Dusan_Markovic.resources.Data;
using PSW_Dusan_Markovic.resources.model;
using PSW_Dusan_Markovic.resources.service;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSW_Test.service
{
    [TestClass]
    public  class TourServiceTests
    {
        private Mock<UserManager<User>> _userManagerMock;
        private YourDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);
            var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            _context = new YourDbContext(options);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public void GetAllTours_ShouldReturlAllPublishedTours()
        {
            //Arrange
            User tempAuthor = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);            
            {
                _context.Users.Add(tempAuthor);
                _context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                _context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                _context.Tours.Add(tour3);
                _context.SaveChanges();      
                var tourService = new TourService(_context);
                //Act
                var result = tourService.getAllTours();
                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod]
        public void GetUserDraftTour_ShouldReturnUserDraftTours()
        {
            //Arrange
            User tempAuthor = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);            
            {
                _context.Users.Add(tempAuthor);
                _context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                _context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                _context.Tours.Add(tour3);
                _context.SaveChanges();         
                var tourService = new TourService(_context);
                //Act
                var result = tourService.getUserDraftTour(tempAuthor.Id);
                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("Tura1", result[0].Name);
            }
        }

        [TestMethod]
        public void GetUserActiveTour_ShouldReturnUserPublishedTours()
        {
            //Arrange
            User tempAuthor = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);            
            {
                _context.Users.Add(tempAuthor);
                _context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                _context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                _context.Tours.Add(tour3);
                _context.SaveChanges();       
                var tourService = new TourService(_context);
                //Act
                var result = tourService.getUserActiveTour(tempAuthor.Id);
                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual("Tura2", result[0].Name);
                Assert.AreEqual("Tura3", result[1].Name);
            }
        }

        [TestMethod]
        public void GetUserTours_ShouldReturnAllToursForAuthor()
        {
            //Arrange
            User tempAuthor = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);            
            {
                _context.Users.Add(tempAuthor);
                _context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                _context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                _context.Tours.Add(tour3);
                _context.SaveChanges();           
                var tourService = new TourService(_context);
                //Act
                var result = tourService.getUserTours(tempAuthor.Id);
                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(3, result.Count);
                Assert.AreEqual("Tura1", result[0].Name);
                Assert.AreEqual("Tura2", result[1].Name);
                Assert.AreEqual("Tura3", result[2].Name);
            }
        }

        [TestMethod]
        public void GetUserTours_ShouldReturnAllToursForTourist()
        {
            //Arrange
            User tempTourist = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);            
            {
                _context.Users.Add(tempTourist);
                _context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                _context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                _context.Tours.Add(tour3);
                TourPurchase purchase1 = new TourPurchase(tempTourist.Id, tour2.TourId, DateTime.Today);
                _context.TourPurchases.Add(purchase1);
                _context.SaveChanges();          
                var tourService = new TourService(_context);
                //Act
                var result = tourService.getUserTours(tempTourist.Id);
                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("Tura2", result[0].Name);
            }
        }

        [TestMethod]
        public void PurchaseTour_ShouldReturnTrue()
        {
            //Arrange
            User tempTourist = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);            
            {
                _context.Users.Add(tempTourist);
                _context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                _context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                _context.Tours.Add(tour3);
                _context.SaveChanges();            
                var tourService = new TourService(_context);
                //Act
                var result = tourService.purchaseTour(tour2.TourId, tempTourist.Id);
                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(true, result);
                Assert.AreEqual("Tura2", tourService.getUserTours(tempTourist.Id)[0].Name);
            }
        }

        [TestMethod]
        public void PurchaseTour_ShouldReturnFalse()
        {
            //Arrange
            User tempTourist = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);            
            {
                _context.Users.Add(tempTourist);
                _context.Tours.Add(tour1);
                _context.Tours.Add(tour2);
                _context.Tours.Add(tour3);
                _context.SaveChanges();           
                var tourService = new TourService(_context);
                //Act
                var result = tourService.purchaseTour(tour1.TourId, tempTourist.Id);
                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(false, result);
            }
        }

        [TestMethod]
        public void ArchieveTour_ShouldReturnTrue()
        {
            //Arrange
            User tempTourist = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);            
            {
                _context.Users.Add(tempTourist);
                tour1.IsPublished = true;
                tour1.IsDraft = false;
                _context.Tours.Add(tour1);
                _context.SaveChanges();
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsDraft, false);
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsPublished, true);           
                var tourService = new TourService(_context);
                //Act
                var result = tourService.archieveTour(tour1.TourId);
                //Assert
                Assert.AreEqual(true, result);
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsDraft, true);
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsPublished, false);
            }
        }

        [TestMethod]
        public void PublishTour_ShouldReturnTrue()
        {
            //Arrange
            User tempTourist = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);            
            {
                _context.Users.Add(tempTourist);
                KeyPoint kp1 = new KeyPoint("keyPoint1", "desc1", 1, 2, tour1.TourId);
                KeyPoint kp2 = new KeyPoint("keyPoint2", "desc2", 1, 2, tour1.TourId);
                tour1.addKeyPoint(kp1);
                tour1.addKeyPoint(kp2);
                _context.KeyPoints.Add(kp1);
                _context.KeyPoints.Add(kp2);
                _context.Tours.Add(tour1);
                _context.SaveChanges();
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsDraft, true);
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsPublished, false);         
                var tourService = new TourService(_context);
                //Act
                var result = tourService.publishTour(tour1.TourId);
                //Assert
                Assert.AreEqual(true, result);
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).KeyPoints.Count, 2);
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsDraft, false);
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsPublished, true);
            }
        }

        [TestMethod]
        public void PublishTour_ShouldReturnFalse()
        {
            //Arrange
            User tempTourist = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);            
            {
                _context.Users.Add(tempTourist);
                KeyPoint kp1 = new KeyPoint("keyPoint1", "desc1", 1, 2, tour1.TourId);
                tour1.addKeyPoint(kp1);
                _context.KeyPoints.Add(kp1);
                _context.Tours.Add(tour1);
                _context.SaveChanges();
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsDraft, true);
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsPublished, false);
                var tourService = new TourService(_context);
                //Act
                var result = tourService.publishTour(tour1.TourId);
                //Assert
                Assert.AreEqual(false, result);
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).KeyPoints.Count, 1);
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsDraft, true);
                Assert.AreEqual(_context.Tours.Find(tour1.TourId).IsPublished, false);
            }
        }

        [TestMethod]
        public void GetRecommendedTours_ReturnTours()
        {
            //Arrange

            Interest interest1 = new Interest(EnumInterest.SPIRITUAL);
            Interest interest2 = new Interest(EnumInterest.ADVENTURE);
            List<Interest> interests1 = new List<Interest>();
            List<Interest> interests2 = new List<Interest>();
            interests1.Add(interest1);
            interests2.Add(interest2);  
            interests2.Add(interest1);
            User tempTourist = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST, interests1);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id, interests1);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id, interests2);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            tour1.IsPublished = true;
            tour2.IsPublished = true;
            tour3.IsPublished = true;            
            {
                _context.Interests.Add(interest1);
                _context.Interests.Add(interest2);
                _context.Tours.Add(tour1);
                _context.Tours.Add(tour2);
                _context.Tours.Add(tour3);
                _context.Users.Add(tempTourist);              
                _context.UserInterests.Add(new UserInterest(tempTourist.Id, interest1.InterestValue));
                _context.TourInterests.Add(new TourInterest(tour1.TourId, interest1.InterestValue));
                _context.TourInterests.Add(new TourInterest(tour2.TourId, interest1.InterestValue));
                _context.TourInterests.Add(new TourInterest(tour2.TourId, interest2.InterestValue));
                _context.SaveChanges();
                Assert.AreEqual(_context.UserInterests.Count(), 1);
                var tourService = new TourService(_context);
                //Act
                var result = tourService.getRecommendedTours(tempTourist.Id);
                //Assert
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual(result[0].TourId, tour1.TourId);
                Assert.AreEqual(result[1].TourId, tour2.TourId);
                Assert.AreEqual(_context.UserInterests.Count(), 1);
                Assert.AreEqual(_context.TourInterests.Count(), 3);
            }
        }

        [TestMethod]
        public void PostTour_ShouldReturnTrue()
        {
            //Arrange
            Interest interest1 = new Interest(EnumInterest.SPIRITUAL);
            Interest interest2 = new Interest(EnumInterest.ADVENTURE);
            List<Interest> interests1 = new List<Interest>();
            interests1.Add(interest1);
            interests1.Add(interest2);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id, interests1);                        
            {
                _context.Interests.Add(interest1);
                _context.Interests.Add(interest2);
                _context.Users.Add(tempAuthor);
                _context.SaveChanges();
                var tourService = new TourService(_context);
                //Act
                var result = tourService.postTour(tour1);
                //Assert
                Assert.AreEqual(true, result);
                var tourFoud = _context.Tours.Find(tour1.TourId);
                Assert.AreEqual(tourFoud.Name, tour1.Name);
                Assert.AreEqual(tourFoud.Interests, interests1);
                var ti = _context.TourInterests.ToList();
                Assert.AreEqual(ti.Count, 2);
            }
        }

        [TestMethod]
        public void UpdateTour_ShouldReturnTrue()
        {
            //Arrange
            Interest interest1 = new Interest(EnumInterest.SPIRITUAL);
            Interest interest2 = new Interest(EnumInterest.ADVENTURE);
            List<Interest> interests1 = new List<Interest>();
            interests1.Add(interest1);
            interests1.Add(interest2);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id, interests1);
            {
                _context.Interests.Add(interest1);
                _context.Interests.Add(interest2);
                _context.Users.Add(tempAuthor);
                _context.SaveChanges();
                var tourService = new TourService(_context);
                //Act
                tourService.postTour(tour1);
                tour1.Name = "Nova tura";
                var result = tourService.updateTour(tour1);
                //Assert
                Assert.AreEqual(true, result);
                var t = _context.Tours.Find(tour1.TourId);
                Assert.AreEqual(t.Name, "Nova tura");
            }
        }
        [TestMethod]
        public void UpdateTour_ShouldReturnFalse()
        {
            //Arrange
            Interest interest1 = new Interest(EnumInterest.SPIRITUAL);
            Interest interest2 = new Interest(EnumInterest.ADVENTURE);
            List<Interest> interests1 = new List<Interest>();
            interests1.Add(interest1);
            interests1.Add(interest2);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id, interests1);
            Tour tour2 = new Tour("Tura2", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id, interests1);         
            {
                _context.Interests.Add(interest1);
                _context.Interests.Add(interest2);
                _context.Users.Add(tempAuthor);
                _context.SaveChanges();
                var tourService = new TourService(_context);
                //Act
                tourService.postTour(tour1);
                tour1.Name = "Nova tura";
                var result = tourService.updateTour(tour2);
                //Assert
                Assert.AreEqual(false, result);

            }
        }
    }
}
