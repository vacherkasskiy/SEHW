namespace Auth.Requests.Account;

/// <summary>
/// Модель для логина пользователя
/// </summary>
/// <param name="Email">Электронный адрес</param>
/// <param name="Password">Пароль</param>
public record LoginRequest(string Email, string Password);