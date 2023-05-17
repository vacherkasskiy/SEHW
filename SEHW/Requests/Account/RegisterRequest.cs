namespace SEHW.Requests.Account;

public record RegisterRequest(string Username, string Email, string Password, string Role);