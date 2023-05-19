namespace Shop.Requests.Dish;

public record EditDishRequest(string Signature, int DishId, string Name, string? Description, decimal Price, int Quantity);