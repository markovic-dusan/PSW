using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSW_Dusan_Markovic.resources.model;
using PSW_Dusan_Markovic.resources.service;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;


[TestClass]
public class LoginServiceTests
{
    private Mock<UserManager<User>> _userManagerMock;
    private Mock<SignInManager<User>> _signInManagerMock;
    private Mock<IConfiguration> _configurationMock;
    private LoginService _loginService;

    [TestInitialize]
    public void Setup()
    {
        _userManagerMock = new Mock<UserManager<User>>( new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object);

        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object);


        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.SetupGet(x => x["Jwt:Key"]).Returns("0w0J3Mo1$3cReT");
        _configurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns("Issuer");
        _configurationMock.SetupGet(x => x["Jwt:ExpireMinutes"]).Returns("30");

        _loginService = new LoginService(_userManagerMock.Object, _signInManagerMock.Object, _configurationMock.Object);
    }

    [TestMethod]
    public async Task AuthenticateAsync_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var validUser = new User("valid", "validpassword", "Dragan", "Deagic", "gagi@example.com", UserType.TOURIST);

        _userManagerMock.Setup(m => m.FindByNameAsync(validUser.UserName)).ReturnsAsync(validUser);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
        _userManagerMock.Setup(m => m.GetRolesAsync(validUser)).ReturnsAsync(new List<string> { "TOURIST" });

        var loginRequest = new LoginRequest { LoginUserName = validUser.UserName, LoginPassword = validUser.Password };

        // Act
        var token = await _loginService.AuthenticateAsync(loginRequest);

        // Assert
        Assert.IsNotNull(token, "Token != null");
    }

    [TestMethod]
    public async Task AuthenticateAsync_InvalidCredentials_ReturnsNull()
    {
        // Arrange
        var invalidUser = new User("invalid", "password", "Dragan", "Dragic", "gagi@example.com", UserType.TOURIST);

        _userManagerMock.Setup(m => m.FindByNameAsync(invalidUser.UserName))
            .ReturnsAsync((User)null);

        var loginRequest = new LoginRequest {LoginUserName = invalidUser.UserName, LoginPassword = "invalidpassword" };

        // Act
        var token = await _loginService.AuthenticateAsync(loginRequest);

        // Assert
        Assert.IsNull(token, "Token == null");
    }
}