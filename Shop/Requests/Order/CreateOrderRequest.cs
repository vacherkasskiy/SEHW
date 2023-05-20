using Shop.Models;

namespace Shop.Requests.Order;

/// <summary>
/// Модель для создания заказа
/// </summary>
/// <param name="Signature">Подпись jwt токена</param>
/// <param name="Dishes">Список желаемых блюд</param>
/// <param name="SpecialRequests">Особые пожелания</param>
public record CreateOrderRequest(string Signature, DishModel[] Dishes, string SpecialRequests);