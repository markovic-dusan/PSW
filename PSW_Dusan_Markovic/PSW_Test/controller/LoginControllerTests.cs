using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PSW_Dusan_Markovic.resources.Data;
using PSW_Dusan_Markovic.resources.model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[TestClass]
public class LoginControllerIntegrationTests
{
    private YourDbContext _dbContext;
    private LoginController _loginController;
    private Mock<UserManager<User>> _userManagerMock;


    [TestInitialize]
    public void Initialize()
    {
        var options = new DbContextOptionsBuilder<YourDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase2")
            .Options;

        _dbContext = new YourDbContext(options);

        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object
        );

        var signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object
        );

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.SetupGet(x => x["Jwt:Key"]).Returns("0w0J3Mo1$3cReT");
        configurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns("Issuer");
        configurationMock.SetupGet(x => x["Jwt:ExpireMinutes"]).Returns("30");

        var loginService = new LoginService(_userManagerMock.Object, signInManagerMock.Object, configurationMock.Object);
        _loginController = new LoginController(loginService);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var validLoginRequest = new LoginRequest { LoginUserName = "valid", LoginPassword = "validPassword" };
        var validUser = new User("valid", "validpassword", "Dragan", "Deagic", "gagi@example.com", UserType.TOURIST);

        _userManagerMock.Setup(m => m.FindByNameAsync(validUser.UserName)).ReturnsAsync(validUser);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
        _userManagerMock.Setup(m => m.GetRolesAsync(validUser)).ReturnsAsync(new List<string> { "TOURIST" });

        // Act
        var result = await _loginController.Login(validLoginRequest);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var tokenObject = ((ObjectResult)result).Value;
        Assert.IsNotNull(tokenObject);

        var token = tokenObject.GetType().GetProperty("Token")?.GetValue(tokenObject);
        Assert.IsNotNull(token);

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token.ToString()) as JwtSecurityToken;

        Assert.IsTrue(jsonToken?.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "TOURIST"));
    }

    [TestMethod]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var invalidLoginRequest = new LoginRequest { LoginUserName = "invalidUserName", LoginPassword = "invalidPassword" };

        // Act
        var result = await _loginController.Login(invalidLoginRequest);

        // Assert
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
    }
}