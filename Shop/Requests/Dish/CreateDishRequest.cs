namespace Shop.Requests.Dish;

/// <summary>
/// Модель для создания блюда
/// </summary>
/// <param name="Signature">Подпись jwt токена</param>
/// <param name="Name">Имя блюда</param>
/// <param name="Description">Описание блюда</param>
/// <param name="Price">Цена блюда</param>
/// <param name="Quantity">Количество блюд</param>
public record CreateDishRequest(string Signature, string Name, string? Description, decimal Price, int Quantity);