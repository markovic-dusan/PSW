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
    private JwtSecurityTokenHandler _handler;
    private TokenValidationParameters _validationParameters;


    public LoginService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _handler = new JwtSecurityTokenHandler();
        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"].PadRight(64)))
        };
    }

    public async Task<string> AuthenticateAsync(LoginRequest loginRequest)
    {
        Console.WriteLine($"Authenticating user: {loginRequest.LoginUserName}");
        var user = await _userManager.FindByNameAsync(loginRequest.LoginUserName);
        //check if user is blocked
        if(user.IsBlocked)
        {
            return null;
        }
        

        if (user == null)
        {
            Console.WriteLine($"User {loginRequest.LoginUserName} not found.");
            return null;
        }

        var passwordCheckResult = await _userManager.CheckPasswordAsync(user, loginRequest.LoginPassword);

        if (!passwordCheckResult)
        {
            Console.WriteLine($"Invalid password for user: {loginRequest.LoginUserName}");
            return null;
        }

        var claims = new List<Claim>();

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

        if (roles != null)
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }
        else
        {
            Console.WriteLine("Roles is null");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"].PadRight(64)));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
            signingCredentials: credentials
        );
        Console.WriteLine("sssssssssssssssssssss");

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool authorize(string token, UserType userType, UserType userType2 = UserType.ADMIN, UserType userType3 = UserType.ADMIN)
    {
        if(token == null || token == "")
        {
            return false;
        } 
        SecurityToken validatedToken;
        var principal = _handler.ValidateToken(token, _validationParameters, out validatedToken);
        var userRole = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        if (userRole.ToString() != userType.ToString() && userRole.ToString() != userType2.ToString() && userRole.ToString() != userType3.ToString())
        {
            return false;
        }
        return true;
    }
}