﻿using Microsoft.AspNetCore.Identity;
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
        Console.WriteLine($"Authenticating user: {loginRequest.LoginUserName}");
        var user = await _userManager.FindByNameAsync(loginRequest.LoginUserName);

        

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

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}