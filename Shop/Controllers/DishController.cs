using Data;
using Data.Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Requests;
using Shop.Requests.Dish;

namespace Shop.Controllers;

[ApiController]
[Route("[controller]")]
public class DishController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public DishController(ApplicationDbContext db)
    {
        _db = db;
    }

    private async Task<bool> CheckAccess(string signature)
    {
        var session = await _db
            .Sessions
            .FirstOrDefaultAsync(x => x.SessionToken == signature);

        if (session == null) return false;

        var user = await _db.Users.FindAsync(session.UserId);

        return user!.Role == RoleTypes.Roles[2];
    }

    [HttpGet]
    [Route("/dishes/get_dishes")]
    public IEnumerable<Dish> GetDishes()
    {
        var dishes = _db.Dishes.Where(x => x.IsAvailable).ToArray();
        return dishes;
    }

    [HttpPost]
    [Route("/dishes/create")]
    public async Task<IActionResult> CreateDish([FromForm] CreateDishRequest request)
    {
        if (!await CheckAccess(request.Signature))
            return StatusCode(StatusCodes.Status400BadRequest, "Access denied (may be you're not manager)");

        var dish = new Dish
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            IsAvailable = request.Quantity > 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _db.Dishes.AddAsync(dish);
        await _db.SaveChangesAsync();
        return StatusCode(StatusCodes.Status200OK, "Dish successfully created");
    }

    [HttpPost]
    [Route("/dishes/delete")]
    public async Task<IActionResult> DeleteDish([FromForm] DeleteDishRequest request)
    {
        if (!await CheckAccess(request.Signature))
            return StatusCode(StatusCodes.Status400BadRequest, "Access denied (may be you're not manager)");

        var dish = await _db.Dishes.FindAsync(request.DishId);
        if (dish == null) return StatusCode(StatusCodes.Status400BadRequest, "Wrong dish id");

        _db.Dishes.Remove(dish);
        await _db.SaveChangesAsync();
        return StatusCode(StatusCodes.Status200OK, "Dish successfully deleted");
    }

    [HttpPatch]
    [Route("/dishes/edit")]
    public async Task<IActionResult> EditDish([FromForm] EditDishRequest request)
    {
        if (!await CheckAccess(request.Signature))
            return StatusCode(StatusCodes.Status400BadRequest, "Access denied (may be you're not manager)");

        var dish = await _db.Dishes.FindAsync(request.DishId);
        if (dish == null) return StatusCode(StatusCodes.Status400BadRequest, "Wrong dish id");

        dish.Name = request.Name.Length == 0 ? dish.Name : request.Name;
        dish.Description = request.Description ?? dish.Description;
        dish.Price = request.Price <= 0 ? dish.Price : request.Price;
        dish.Quantity = request.Quantity <= 0 ? dish.Quantity : request.Quantity;
        dish.IsAvailable = dish.Quantity > 0;
        dish.UpdatedAt = DateTime.UtcNow;

        _db.Dishes.Update(dish);
        await _db.SaveChangesAsync();
        return StatusCode(StatusCodes.Status200OK, "Dish successfully patched");
    }
}