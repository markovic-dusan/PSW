using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PSW_Dusan_Markovic.resources.model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class LoginService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public LoginService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<string> AuthenticateAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByNameAsync(loginRequest.Username);

        // Pogrešan username/password
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            return null;
        }

        var claims = new List<Claim>();

        // Provera null vrednosti pre dodavanja Claims
        if (!string.IsNullOrEmpty(user.UserName))
        {
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
        }
        else
        {
            Console.WriteLine("UserName is null or empty");
        }

        if (!string.IsNullOrEmpty(user.Id))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        }
        else
        {
            Console.WriteLine("UserId is null or empty");
        }

        var roles = await _userManager.GetRolesAsync(user);

        // Provera null vrednosti pre dodavanja Claims
        if (roles != null)
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }
        else
        {
            Console.WriteLine("Roles is null");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}