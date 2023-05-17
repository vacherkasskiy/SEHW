using Data.Data;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers;

[ApiController]
[Route("[controller]")]
public class ShopController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    
    public ShopController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    [HttpGet(Name = "GetWeatherForecast")]
    public string Get()
    {
        return _db.Users.Find(3)!.Email;
    }
}