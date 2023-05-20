namespace Shop.Requests.Dish;

/// <summary>
/// Модель для удаления блюда
/// </summary>
/// <param name="Signature">Подпись jwt токена</param>
/// <param name="DishId">Уникальный ключ блюда</param>
public record DeleteDishRequest(string Signature, int DishId);