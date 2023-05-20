namespace Shop.Requests.Order;

/// <summary>
/// Метод для получения информации о заказе
/// </summary>
/// <param name="OrderId">Уникальный ключ заказа</param>
public record GetInfoRequest(int OrderId);