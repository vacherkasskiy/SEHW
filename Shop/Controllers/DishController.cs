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
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    private readonly ApplicationDbContext _db;

    public DishController(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Метод, проверяющий является ли пользователь менеджером или нет
    /// </summary>
    /// <param name="signature">Подпись jwt токена</param>
    /// <returns>true, если пользователь является менеджером, иначе - false</returns>
    private async Task<bool> CheckAccess(string signature)
    {
        var session = await _db
            .Sessions
            .FirstOrDefaultAsync(x => x.SessionToken == signature && x.ExpiresAt > DateTime.UtcNow);

        if (session == null) return false;

        var user = await _db.Users.FindAsync(session.UserId);

        return user!.Role == RoleTypes.Roles[2];
    }

    /// <summary>
    /// Метод, возвращающий список всех доступных блюд в ресторане
    /// </summary>
    /// <returns>Все доступные блюда</returns>
    [HttpGet]
    [Route("/dishes/get_dishes")]
    public IEnumerable<Dish> GetDishes()
    {
        var dishes = _db.Dishes.Where(x => x.IsAvailable).ToArray();
        return dishes;
    }

    /// <summary>
    /// Метод для создания нового блюда
    /// </summary>
    /// <param name="request">Модель нового блюда</param>
    /// <returns>HTTP код (успех\провал)</returns>
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

    /// <summary>
    /// Метод для удаления блюда из меню
    /// </summary>
    /// <param name="request">Модель для удаления блюда</param>
    /// <returns>HTTP код (успех\провал)</returns>
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

    /// <summary>
    /// Метод для корректирования\изменения уже существующего блюда
    /// </summary>
    /// <param name="request">Модель для изменения блюда</param>
    /// <returns>HTTP код (успех\провал)</returns>
    [HttpPatch]
    [Route("/dishes/edit")]
    public async Task<IActionResult> EditDish([FromForm] EditDishRequest request)
    {
        if (!await CheckAccess(request.Signature))
            return StatusCode(StatusCodes.Status400BadRequest, "Access denied (may be you're not manager)");

        var dish = await _db.Dishes.FindAsync(request.DishId);
        if (dish == null) return StatusCode(StatusCodes.Status400BadRequest, "Wrong dish id");

        // здесь блюду будет присвоено старое значение, если новое не введено
        // или введено некорректно.
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