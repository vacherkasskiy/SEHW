using Data.Data;

namespace Shop.HostedServices;

public sealed class ChangeOrderStatusHostedService : BackgroundService
{
    private readonly ApplicationDbContext _db;

    public ChangeOrderStatusHostedService(
        ApplicationDbContext db)
    {
        _db = db;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var pendingDishes = _db
                .Orders
                .Where(x => x.Status == "Pending" && x.UpdatedAt <= DateTime.UtcNow.AddMinutes(1))
                .ToArray();

            foreach (var pendingDish in pendingDishes)
            {
                pendingDish.Status = "Being prepared";
                pendingDish.UpdatedAt = DateTime.UtcNow;
            }

            var preparedDishes = _db
                .Orders
                .Where(x => x.Status == "Being prepared" && x.UpdatedAt <= DateTime.UtcNow.AddMinutes(2))
                .ToArray();

            foreach (var preparedDish in preparedDishes)
            {
                preparedDish.Status = "Ready";
                preparedDish.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync(token);
            await Task.Delay(TimeSpan.FromSeconds(15), token);
        }
    }
}