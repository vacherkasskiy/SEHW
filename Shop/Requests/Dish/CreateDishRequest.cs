namespace Shop.Requests;

public record CreateDishRequest(string Signature, string Name, string? Description, decimal Price, int Quantity);