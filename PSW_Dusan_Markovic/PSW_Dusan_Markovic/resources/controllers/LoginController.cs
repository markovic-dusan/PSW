using Microsoft.AspNetCore.Mvc;
using PSW_Dusan_Markovic.resources.model;

[Route("api/auth")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly LoginService _loginService;

    public LoginController(LoginService loginService)
    {
        _loginService = loginService;
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
        Console.WriteLine($"User {loginRequest.LoginUserName} authenticated successfully.");
        return Ok(new { Token = token });
    }
}