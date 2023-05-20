namespace Auth.Requests.Account;

/// <summary>
/// Модель для регистрации пользователя
/// </summary>
/// <param name="Username">Никнейм</param>
/// <param name="Email">Электронный адрес</param>
/// <param name="Password">Пароль</param>
/// <param name="Role">Роль</param>
public record RegisterRequest(string Username, string Email, string Password, string Role);