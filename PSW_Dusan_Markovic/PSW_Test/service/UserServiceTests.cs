using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PSW_Dusan_Markovic.resources.model;
using PSW_Dusan_Markovic.resources.service;
using PSW_Dusan_Markovic.resources.Data;

using System.Collections.Generic;

[TestClass]
public class UserServiceTests
{
    [TestMethod]
    public void GetAllUsers_ShouldReturnAllUsers()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<YourDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new YourDbContext(options))
        {
            context.Users.Add(new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST));
            context.SaveChanges();
        }

        using (var context = new YourDbContext(options))
        {
            var userService = new UserService(context);

            // Act
            var result = userService.getAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }
    }

    [TestMethod]
    public void RegisterUser_NewUser_SuccessfullyRegistered()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<YourDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new YourDbContext(options))
        {
            var userService = new UserService(context);

            var newUser = new User("newuser", "password", "Jane", "Doe", "jane22@example.com", UserType.TOURIST);

            // Act
            var result = userService.registerUser(newUser);

            // Assert
            Assert.IsTrue(result, "User registration should be successful");

            var registeredUser = context.Users.FirstOrDefault(u => u.Username == "newuser");
            Assert.IsNotNull(registeredUser, "User should be saved in the database");
        }
    }

    [TestMethod]
    public void RegisterUser_UserWithExistingEmail_RegistrationFails()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<YourDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new YourDbContext(options))
        {
            context.Users.Add(new User("existinguser", "password", "John", "Doe", "john@example.com", UserType.TOURIST));
            context.SaveChanges();

            var userService = new UserService(context);

            var existingUser = new User("newuser", "password", "Jane", "Doe", "john@example.com", UserType.TOURIST);

            // Act
            var result = userService.registerUser(existingUser);

            // Assert
            Assert.IsFalse(result, "User registration should fail");
        }
    }

    [TestMethod]
    public void DeleteUser_ExistingUser_DeletionSuccessful()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<YourDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new YourDbContext(options))
        {
            context.Users.Add(new User("existinguser", "password", "John", "Doe", "john@example.com", UserType.TOURIST));
            context.SaveChanges();
        }

        using (var context = new YourDbContext(options))
        {
            var userService = new UserService(context);

            var userIdToDelete = 1; 

            // Act
            var result = userService.deleteUser(userIdToDelete);

            // Assert
            Assert.IsTrue(result, "User deletion should be successful");

            var deletedUser = context.Users.Find(userIdToDelete);
            Assert.IsNull(deletedUser, "User should be deleted from the database");
        }
    }

    [TestMethod]
    public void DeleteUser_ExistingUser_DeletionFailed()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<YourDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new YourDbContext(options))
        {
            var userService = new UserService(context);

            var userIdToDelete = 5; // nepostojeci Id

            // Act
            var result = userService.deleteUser(userIdToDelete);

            // Assert
            Assert.IsFalse(result, "User deletion should fail");
        }
    }



}