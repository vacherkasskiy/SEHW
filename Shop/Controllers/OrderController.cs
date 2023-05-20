using System.Text.Json;
using Data.Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Requests;
using Shop.Requests.Order;

namespace Shop.Controllers;

[ApiController]
[Route("[controller]")]
public class ShopController : ControllerBase
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    private readonly ApplicationDbContext _db;

    public ShopController(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Получает информацию о заказе по уникальному ключу
    /// </summary>
    /// <param name="request">Уникальный ключ</param>
    /// <returns>Информацию о заказе</returns>
    [HttpPost]
    [Route("/order/get_info")]
    public async Task<IActionResult> GetInfo([FromForm] GetInfoRequest request)
    {
        var order = await _db.Orders.FindAsync(request.OrderId);
        if (order == null) return StatusCode(StatusCodes.Status400BadRequest, "Wrong order id");

        return StatusCode(StatusCodes.Status200OK, JsonSerializer.Serialize(order));
    }

    /// <summary>
    /// Метод для создания нового заказа
    /// </summary>
    /// <param name="request">Модель для создания нового заказа</param>
    /// <returns>HTTP код (успех\провал)</returns>
    [HttpPost]
    [Route("/order/create_order")]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        if (await _db
                .Sessions
                .FirstOrDefaultAsync(x => x.SessionToken == request.Signature && x.ExpiresAt > DateTime.UtcNow)
            == null)
            return StatusCode(StatusCodes.Status400BadRequest, "Wrong signature");

        if (request.Dishes.Any(x => _db.Dishes.Find(x.DishId) == null))
            return StatusCode(StatusCodes.Status400BadRequest, "Wrong dish id(-s)");

        if (request.Dishes.Any(x => _db.Dishes.Find(x.DishId)!.Quantity < x.Quantity))
            return StatusCode(StatusCodes.Status400BadRequest, "Wrong quantity provided");

        var userId = (await _db.Sessions.FirstAsync(x => x.SessionToken == request.Signature)).UserId;
        var order = new Order
        {
            UserId = userId,
            Status = "Pending",
            SpecialRequests = request.SpecialRequests,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // добавляем и сохраняем заказ
        await _db.Orders.AddAsync(order);
        await _db.SaveChangesAsync();
        var orderId = order.Id;

        // добавляем все заказанные блюда
        foreach (var dish in request.Dishes)
        {
            var foundDish = (await _db.Dishes.FindAsync(dish.DishId))!;
            foundDish.Quantity -= dish.Quantity; // уменьшаем кол-во блюд
            if (foundDish.Quantity == 0) foundDish.IsAvailable = false; // при необходимости меняем доступность

            var orderDish = new OrderDish
            {
                OrderId = orderId,
                DishId = dish.DishId,
                Quantity = dish.Quantity,
                Price = (await _db.Dishes.FindAsync(dish.DishId))!.Price
            };

            await _db.OrderDishes.AddAsync(orderDish);
        }

        // сохраняем изменения ещё раз
        await _db.SaveChangesAsync();

        return StatusCode(StatusCodes.Status200OK, "Order successfully created");
    }
}