using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using PSW_Dusan_Markovic.resources.model;

[Route("api/auth")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly LoginService _loginService;
    private readonly UserService _userService;


    public LoginController(LoginService loginService, UserService userService)
    {
        _loginService = loginService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        Console.WriteLine($"Received login request for user: {loginRequest.LoginUserName}");
        var token = await _loginService.AuthenticateAsync(loginRequest);
        if (token == null)
        {
            Console.WriteLine($"Authentication failed for user: {loginRequest.LoginUserName}");
            return Unauthorized("Invalid UserName or password.");
        }
        var user = _userService.getUserByUsername(loginRequest.LoginUserName);
        Console.WriteLine($"User {loginRequest.LoginUserName} authenticated successfully.");
        var loginInfo = new LoginInfo(token, user);
        return Ok(new { Token = loginInfo.Token, Username = loginInfo.Username, Role = loginInfo.Role, UserId = loginInfo.UserId, Email = loginInfo.Email });
    }
}