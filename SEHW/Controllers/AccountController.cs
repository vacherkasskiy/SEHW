using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEHW.Data;
using SEHW.Models;
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
    
    private bool IsValidEmail(string email)
    {
        try
        {
            var m = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
    
    [HttpPost]
    [Route("/account/register")]
    public async Task<IActionResult> Register([FromForm]RegisterRequest request)
    {
        if (!IsValidEmail(request.Email))
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Wrong email");
        }

        if (await _db.Users.FirstOrDefaultAsync(x => x.Email == request.Email) != null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "User with such email already exists");
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Role = "user",
            PasswordHash = request.Password
        };
        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return StatusCode(StatusCodes.Status200OK, "User registered successfully");
    }
}