using Data.Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Requests;

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

    [HttpGet]
    [Route("/shop/get_dishes")]
    public IEnumerable<Dish> GetDishes()
    {
        var dishes = _db.Dishes.ToArray();
        return dishes;
    }

    [HttpPost]
    [Route("/shop/create_order")]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        if (await _db
                .Sessions
                .FirstOrDefaultAsync(x => x.SessionToken == request.Signature && x.ExpiresAt > DateTime.UtcNow)
            == null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Wrong signature");
        }

        if (request.Dishes.Any(x => _db.Dishes.Find(x.DishId) == null))
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Wrong dish id(-s)");
        }
        
        if (request.Dishes.Any(x => _db.Dishes.Find(x.DishId)!.Quantity < x.Quantity))
        {
            return StatusCode(StatusCodes.Status400BadRequest, "Wrong quantity provided");
        }

        var userId = (await _db.Sessions.FirstAsync(x => x.SessionToken == request.Signature)).UserId;
        var order = new Order
        {
            UserId = userId,
            Status = "Not ready",
            SpecialRequests = request.SpecialRequests,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _db.Orders.AddAsync(order);
        await _db.SaveChangesAsync();
        var orderId = order.Id;
        
        foreach (var dish in request.Dishes)
        {
            (await _db.Dishes.FindAsync(dish.DishId))!.Quantity -= dish.Quantity;
            
            var orderDish = new OrderDish
            {
                OrderId = orderId,
                DishId = dish.DishId,
                Quantity = dish.Quantity,
                Price = (await _db.Dishes.FindAsync(dish.DishId))!.Price
            };
            
            await _db.OrderDishes.AddAsync(orderDish);
        }

        await _db.SaveChangesAsync();

        return StatusCode(StatusCodes.Status200OK, "Order successfully created");
    }
}