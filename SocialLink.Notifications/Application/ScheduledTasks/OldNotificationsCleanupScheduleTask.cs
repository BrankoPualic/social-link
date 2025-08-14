using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Serilog;
using SocialLink.Notifications;
using SocialLink.Notifications.Domain;

namespace SocialLink.Notifications.Application.ScheduledTasks;

internal class OldNotificationsCleanupScheduleTask(IServiceProvider serviceProvider, ILogger logger) : IHostedService, IDisposable
{
	private Timer _timer;

	public Task StartAsync(CancellationToken ct)
	{
		_timer = new Timer(Cleanup, state: null, TimeSpan.Zero, TimeSpan.FromDays(1));

		return Task.CompletedTask;
	}

	private void Cleanup(object state)
	{
		using var scope = serviceProvider.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<INotificationMongoContext>();

		var deadline = DateTime.UtcNow.AddDays(-30);

		var builder = Builders<Notification>.Filter;
		var filter = builder.Lt(_ => _.CreatedOn, deadline);
		try
		{
			var result = db.Notifications.DeleteMany(filter);
			if (result.DeletedCount > 0)
			{
				logger.Information("Deleted {count} notifications older than {deadline}.", result.DeletedCount, deadline);
			}
			else
			{
				logger.Information("No old notification deleted.");
			}
		}
		catch (Exception ex)
		{
			logger.Error(ex, $"EXCEPTION: Old Notifications Cleanup Schedule Task\n----------\n{ex.Message}");
		}
	}

	public Task StopAsync(CancellationToken ct)
	{
		_timer?.Change(Timeout.Infinite, 0);
		return Task.CompletedTask;
	}

	public void Dispose()
	{
		_timer?.Dispose();
	}
}