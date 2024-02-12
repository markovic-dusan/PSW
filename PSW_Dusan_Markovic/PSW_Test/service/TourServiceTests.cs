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

        [TestInitialize]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);

        }

        [TestMethod]
        public void GetAllTours_ShouldReturlAllPublishedTours()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            User tempAuthor = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);
            using (var context = new YourDbContext(options))
            {
                context.Users.Add(tempAuthor);
                context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                context.Tours.Add(tour3);
                context.SaveChanges();
            }

            using (var context = new YourDbContext(options))
            {
                var tourService = new TourService(context);

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
            var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            User tempAuthor = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);
            using (var context = new YourDbContext(options))
            {
                context.Users.Add(tempAuthor);
                context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                context.Tours.Add(tour3);
                context.SaveChanges();
            }

            using (var context = new YourDbContext(options))
            {
                var tourService = new TourService(context);

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
            var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            User tempAuthor = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);
            using (var context = new YourDbContext(options))
            {
                context.Users.Add(tempAuthor);
                context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                context.Tours.Add(tour3);
                context.SaveChanges();
            }

            using (var context = new YourDbContext(options))
            {
                var tourService = new TourService(context);

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
            var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            User tempAuthor = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.AUTHOR);
            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);
            using (var context = new YourDbContext(options))
            {
                context.Users.Add(tempAuthor);
                context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                context.Tours.Add(tour3);
                context.SaveChanges();
            }

            using (var context = new YourDbContext(options))
            {
                var tourService = new TourService(context);

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
            var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            User tempTourist = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);

            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);
            using (var context = new YourDbContext(options))
            {
                context.Users.Add(tempTourist);
                context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                context.Tours.Add(tour3);
                TourPurchase purchase1 = new TourPurchase(tempTourist.Id, tour2.TourId, DateTime.Today);
                context.TourPurchases.Add(purchase1);
                context.SaveChanges();
            }

            using (var context = new YourDbContext(options))
            {
                var tourService = new TourService(context);

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
            var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            User tempTourist = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);

            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);
            using (var context = new YourDbContext(options))
            {
                context.Users.Add(tempTourist);
                context.Tours.Add(tour1);
                tour2.IsPublished = true;
                tour2.IsDraft = false;
                context.Tours.Add(tour2);
                tour3.IsPublished = true;
                tour3.IsDraft = false;
                context.Tours.Add(tour3);
                context.SaveChanges();
            }

            using (var context = new YourDbContext(options))
            {
                var tourService = new TourService(context);

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
            var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            User tempTourist = new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST);
            User tempAuthor = new User("newuser2", "password", "Jane", "Doe", "jane2@example.com", UserType.AUTHOR);

            Tour tour1 = new Tour("Tura1", "Opis ture 1...", EnumTourDifficulty.HARD, "kategorija", 20, tempAuthor.Id);
            Tour tour2 = new Tour("Tura2", "Opis ture 2...", EnumTourDifficulty.INTERMEDIATE, "kategorija", 20, tempAuthor.Id);
            Tour tour3 = new Tour("Tura3", "Opis ture 3...", EnumTourDifficulty.EASY, "kategorija", 20, tempAuthor.Id);
            using (var context = new YourDbContext(options))
            {
                context.Users.Add(tempTourist);
                context.Tours.Add(tour1);
                context.Tours.Add(tour2);
                context.Tours.Add(tour3);
                context.SaveChanges();
            }

            using (var context = new YourDbContext(options))
            {
                var tourService = new TourService(context);

                //Act
                var result = tourService.purchaseTour(tour1.TourId, tempTourist.Id);

                //Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(false, result);
            }
        }
    }
}
