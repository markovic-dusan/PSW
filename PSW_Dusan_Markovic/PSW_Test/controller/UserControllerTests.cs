using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSW_Dusan_Markovic.resources.Data;
using PSW_Dusan_Markovic.resources.model;
using PSW_Dusan_Markovic.resources.service;
using PSW_Dusan_Markovic.resources.controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

[TestClass]
public class UserControllerIntegrationTests
{
    private YourDbContext _dbContext;
    private UserController _userController;

    [TestInitialize]
    public void Initialize()
    {
        var options = new DbContextOptionsBuilder<YourDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase2")
            .Options;

        _dbContext = new YourDbContext(options);

        _userController = new UserController(new UserService(_dbContext));
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Dispose();
    }

    [TestMethod]
    public void GetAllUsers_ReturnsListOfUsers()
    {
        // Arrange
        _dbContext.Users.AddRange(
            new User("user1", "pass1", "John", "Doe", "john@example.com", UserType.TOURIST),
            new User("user2", "pass2", "Jane", "Doe", "jane@example.com", UserType.TOURIST)
        );
        _dbContext.SaveChanges();

        // Act
        var result = _userController.getUsers();
        var users = (List<User>)((ObjectResult)result.Result).Value;

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult)); // provera da li je status 200 OK
        Assert.IsNotNull(users);
        Assert.AreEqual(2, users.Count);
    }

    [TestMethod]
    public async Task GetUserById_ExistingUser_ReturnsUser()
    {
        // Arrange
        var existingUser = new User("user1", "pass1", "John", "Doe", "john@example.com", UserType.TOURIST);
        _dbContext.Users.Add(existingUser);
        _dbContext.SaveChanges();

        // Act
        var response =  _userController.getUserById(existingUser.UserId);

        // Assert
        Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
        var user = (User)((ObjectResult)response.Result).Value;
        Assert.IsNotNull(user);
        Assert.AreEqual(existingUser.UserId, user.UserId);
    }

    [TestMethod]
    public async Task GetUserById_NonExistingUser_ReturnsBadRequest()
    {
        // Arrange
        var nonExistingUserId = 999; //nepostojeci id

        // Act
        var response =  _userController.getUserById(nonExistingUserId);

        // Assert
        Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
        Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)response.Result).StatusCode);
    }

    [TestMethod]
    public async Task RegisterUser_ValidUser_ReturnsOk()
    {
        // Arrange
        var newUser = new User("newUser", "password", "FirstName", "LastName", "novi@korisnik.com", UserType.TOURIST);

        // Act
        var response =  _userController.registerUser(newUser);

        // Assert
        Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
        var result = (bool)((ObjectResult)response.Result).Value;
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task RegisterUser_DuplicateUser_ReturnsBadRequest()
    {
        // Arrange
        var existingUser = new User("existingUser", "password", "FirstName", "LastName", "scepa@scepa.com", UserType.TOURIST);
        _dbContext.Users.Add(existingUser);
        _dbContext.SaveChanges();

        //isti mejl
        var duplicateUser = new User("existingUser", "password", "NewFirstName", "NewLastName", "scepa@scepa.com", UserType.TOURIST);

        // Act
        var response =  _userController.registerUser(duplicateUser);

        // Assert
        Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
        Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)response.Result).StatusCode);
    }
    [TestMethod]
    public async Task UpdateUser_ValidUser_ReturnsOk()
    {
        // Arrange
        var existingUser = new User("existingUser", "password", "FirstName", "LastName", "existinguser@example.com", UserType.TOURIST);
        _dbContext.Users.Add(existingUser);
        _dbContext.SaveChanges();

        var updatedUser = new User("existingUser", "newpassword", "NewFirstName", "NewLastName", "newemail@example.com", UserType.TOURIST);
        updatedUser.UserId = existingUser.UserId;

        // Act
        var response = _userController.updateUser(updatedUser);

        // Assert
        Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
        var result = (bool)((ObjectResult)response.Result).Value;
        Assert.IsTrue(result);
        Assert.AreEqual(existingUser.Email, "newemail@example.com");
    }

    [TestMethod]
    public async Task UpdateUser_NonexistentUser_ReturnsBadRequest()
    {
        // Arrange
        var nonExistentUser = new User("nonExistentUser", "password", "FirstName", "LastName", "nonexistentuser@example.com", UserType.TOURIST);

        // Act
        var response = _userController.updateUser(nonExistentUser);

        // Assert
        Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
        Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)response.Result).StatusCode);
    }
    [TestMethod]
    public async Task DeleteUser_ExistingUser_ReturnsOk()
    {
        // Arrange
        var existingUser = new User("existingUser", "password", "FirstName", "LastName", "existinguser@example.com", UserType.TOURIST);
        _dbContext.Users.Add(existingUser);
        _dbContext.SaveChanges();

        // Act
        var response = _userController.deleteUser(existingUser.UserId);

        // Assert
        Assert.IsInstanceOfType(response.Result, typeof(OkObjectResult));
        var result = (bool)((ObjectResult)response.Result).Value;
        Assert.IsTrue(result);

        // Verify user is deleted
        Assert.IsNull(_dbContext.Users.Find(existingUser.UserId));
    }

    [TestMethod]
    public async Task DeleteUser_NonexistentUser_ReturnsBadRequest()
    {
        // Arrange
        var nonExistentUserId = 123; // nepostojeci id

        // Act
        var response = _userController.deleteUser(nonExistentUserId);

        // Assert
        Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
        Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)response.Result).StatusCode);
    }
}