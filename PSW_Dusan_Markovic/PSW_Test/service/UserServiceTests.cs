using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PSW_Dusan_Markovic.resources.model;
using PSW_Dusan_Markovic.resources.service;
using PSW_Dusan_Markovic.resources.Data;

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

[TestClass]
public class UserServiceTests
{
    private Mock<UserManager<User>> _userManagerMock = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);

    [TestMethod]
    public void GetAllUsers_ShouldReturnAllUsers()
    {
        // Arrange
        _userManagerMock = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);
        var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
        using (var context = new YourDbContext(options))
        {
            context.Users.Add(new User("newuser", "password", "Jane", "Doe", "jane@example.com", UserType.TOURIST));
            context.SaveChanges();
        }

        using (var context = new YourDbContext(options))
        {
            var userService = new UserService(context, _userManagerMock.Object);

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
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
        using (var context = new YourDbContext(options))
        {
            var userService = new UserService(context, _userManagerMock.Object);
            Interest interest1 = new Interest(EnumInterest.SPIRITUAL);
            Interest interest2 = new Interest(EnumInterest.ADVENTURE);
            context.Interests.Add(interest1);
            context.Interests.Add(interest2);
            List<Interest> interests = new List<Interest>();
            interests.Add(interest1);
            interests.Add(interest2);

            var newUser = new User("newuser2", "password", "Jane", "Doe", "jane22@example.com", UserType.TOURIST, interests);

            // Act
            var result = userService.registerUser(newUser);

            // Assert
            Assert.IsTrue(result, "User registration should be successful");
            Assert.AreEqual(context.UserInterests.Count(), 2);
            Assert.AreEqual(context.Users.Find(newUser.Id).Interests.Count, 2);
            var registeredUser = context.Users.FirstOrDefault(u => u.UserName == "newuser");
            Assert.IsNotNull(registeredUser, "User should be saved in the database");
        }
    }

    [TestMethod]
    public void RegisterUser_UserWithExistingEmail_RegistrationFails()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

        using (var context = new YourDbContext(options))
        {
            context.Users.Add(new User("existinguser", "password", "John", "Doe", "john@example.com", UserType.TOURIST));
            context.SaveChanges();

            var userService = new UserService(context, _userManagerMock.Object);

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
        var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

        User userToDelete = new User("existinguser", "password", "John", "Doe", "john@example.com", UserType.TOURIST);
        using (var context = new YourDbContext(options))
        {
            context.Users.Add(userToDelete);
            context.SaveChanges();
        }

        using (var context = new YourDbContext(options))
        {
            var userService = new UserService(context, _userManagerMock.Object);

            var userIdToDelete = userToDelete.Id; 

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
        var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

        using (var context = new YourDbContext(options))
        {
            var userService = new UserService(context, _userManagerMock.Object);

            var userIdToDelete = "5"; // nepostojeci Id

            // Act
            var result = userService.deleteUser(userIdToDelete);

            // Assert
            Assert.IsFalse(result, "User deletion should fail");
        }
    }

    [TestMethod]
    public void UpdateUser_ExistingUser_SuccessfulUpdate()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
        using (var context = new YourDbContext(options))
        {
            var existingUser = new User("existinguser", "password", "John", "Doe", "john@example.com", UserType.TOURIST);
            context.Users.Add(existingUser);
            context.SaveChanges();
            var userService = new UserService(context, _userManagerMock.Object);
            // Act
            var updatedUser = new User("newusername", "newpassword", "NewName", "NewLastName", "newemail@example.com", UserType.TOURIST)
            {
                Id = existingUser.Id
            };
            var result = userService.updateUser(updatedUser);
            // Assert
            Assert.IsTrue(result, "User update should be successful");
            var retrievedUser = context.Users.Find(existingUser.Id);
            Assert.IsNotNull(retrievedUser, "User should be found in the database");
            Assert.AreEqual("newusername", retrievedUser.UserName);
            Assert.AreEqual("newpassword", retrievedUser.Password);
            Assert.AreEqual("NewName", retrievedUser.Name);
            Assert.AreEqual("NewLastName", retrievedUser.LastName);
            Assert.AreEqual("newemail@example.com", retrievedUser.Email);
        }
    }

    [TestMethod]
    public void UpdateUser_NonExistingUser_UpdateFailed()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<YourDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

        using (var context = new YourDbContext(options))
        {
        }

        using (var context = new YourDbContext(options))
        {
            var userService = new UserService(context, _userManagerMock.Object);

            var nonExistingUserId = "1"; // nepostojeci id

            var updatedUserData = new User("updateduser", "newpassword", "Jane", "Doe", "jane@example.com", UserType.TOURIST);
            updatedUserData.Id = nonExistingUserId; 

            // Act
            var result = userService.updateUser(updatedUserData);

            // Assert
            Assert.IsFalse(result, "User update should fail for non-existing user");

            var updatedUser = context.Users.Find(nonExistingUserId);
            Assert.IsNull(updatedUser, "User should not be updated in the database");
        }
    }



}