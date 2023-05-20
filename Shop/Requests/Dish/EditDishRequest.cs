namespace Shop.Requests.Dish;

/// <summary>
/// Модель для изменения блюда
/// </summary>
/// <param name="Signature">Подпись jwt токена</param>
/// <param name="DishId">Уникальный ключ блюда</param>
/// <param name="Name">Имя блюда</param>
/// <param name="Description">Описание блюда</param>
/// <param name="Price">Цена блюда</param>
/// <param name="Quantity">Количество блюд</param>
public record EditDishRequest(string Signature, int DishId, string Name, string? Description, decimal Price,
    int Quantity);