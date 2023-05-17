using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Data;
using Data.Data;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SEHW.Requests.Account;

namespace SEHW.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    
    public AccountController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            JwtConfig.Issuer,
            JwtConfig.Audience,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost]
    [Route("/account/register")]
    public async Task<IActionResult> Register([FromForm]RegisterRequest request)
    {
        if (await _db.Users.FirstOrDefaultAsync(x => x.Email == request.Email) != null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "User with such email already exists");
        }

        if (!RoleTypes.Roles.Contains(request.Role.ToLower()))
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Role should be in list of {customer, chef, manager}");
        }

        var user = new User
        {
            Username = request.Username.ToLower(),
            Email = request.Email.ToLower(),
            Role = request.Role.ToLower(),
            PasswordHash = request.Password
        };

        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return StatusCode(StatusCodes.Status200OK, $"User registered successfully");
    }
    
    [HttpPost]
    [Route("/account/login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = _db.Users.FirstOrDefault(x => x.Email == request.Email);

        if (user == null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "User with such email already exists");
        }
        
        var token = Generate(user);
        var session = new Session
        {
            UserId = user.Id,
            SessionToken = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };
        
        await _db.Sessions.AddAsync(session);
        await _db.SaveChangesAsync();
        
        return StatusCode(StatusCodes.Status200OK, $"{session.User.Username}");
    }
}