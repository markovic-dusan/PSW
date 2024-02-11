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
        var token = await _loginService.AuthenticateAsync(loginRequest);
        if (token == null)
        {
            return Unauthorized("Invalid UserName or password.");
        }
        return Ok(new { Token = token });
    }
}