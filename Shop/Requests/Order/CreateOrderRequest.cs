using Shop.Models;

namespace Shop.Requests;

public record CreateOrderRequest(string Signature, DishModel[] Dishes, string SpecialRequests);