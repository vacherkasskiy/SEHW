namespace Shop.Models;

/// <summary>
/// Модель блюда для создания заказа
/// </summary>
/// <param name="DishId">Уникальный ключ блюда</param>
/// <param name="Quantity">Количество данного блюда</param>
public record DishModel(int DishId, int Quantity);