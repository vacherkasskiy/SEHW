using Data.Data;

namespace Shop.HostedServices;

public sealed class ChangeOrderStatusHostedService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private Timer? _timer;

    public ChangeOrderStatusHostedService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(DoWork,
            null,
            TimeSpan.Zero,
            TimeSpan.FromMinutes(1));

        return Task.CompletedTask;
    }
    
    private void DoWork(object? state)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
        var pendingDishes = db
            .Orders
            .Where(x => x.Status == "Pending" && x.UpdatedAt.AddMinutes(1) <= DateTime.UtcNow)
            .ToArray();

        foreach (var pendingDish in pendingDishes)
        {
            pendingDish.Status = "Being prepared";
            pendingDish.UpdatedAt = DateTime.UtcNow;
        }

        var preparedDishes = db
            .Orders
            .Where(x => x.Status == "Being prepared" && x.UpdatedAt.AddMinutes(2) <= DateTime.UtcNow)
            .ToArray();

        foreach (var preparedDish in preparedDishes)
        {
            preparedDish.Status = "Ready";
            preparedDish.UpdatedAt = DateTime.UtcNow;
        }

        db.SaveChanges();
    }
    
    public Task StopAsync(CancellationToken stoppingToken)
    {
        _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}