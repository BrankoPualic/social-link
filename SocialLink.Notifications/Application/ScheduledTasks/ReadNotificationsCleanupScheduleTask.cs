using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Serilog;
using SocialLink.Notifications;
using SocialLink.Notifications.Domain;

namespace SocialLink.Notifications.Application.ScheduledTasks;

internal class ReadNotificationsCleanupScheduleTask(IServiceProvider serviceProvider, ILogger logger) : IHostedService, IDisposable
{
	private Timer _timer;

	public Task StartAsync(CancellationToken ct)
	{
		_timer = new Timer(Cleanup, state: null, TimeSpan.Zero, TimeSpan.FromHours(8));

		return Task.CompletedTask;
	}

	private void Cleanup(object state)
	{
		using var scope = serviceProvider.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<INotificationMongoContext>();

		var builder = Builders<Notification>.Filter;
		var filter = builder.Lt(_ => _.IsRead, true);
		try
		{
			var result = db.Notifications.DeleteMany(filter);
			if (result.DeletedCount > 0)
			{
				logger.Information("Deleted {count} read notifications.", result.DeletedCount);
			}
			else
			{
				logger.Information("No read notification deleted.");
			}
		}
		catch (Exception ex)
		{
			logger.Error(ex, $"EXCEPTION: Read Notifications Cleanup Schedule Task\n----------\n{ex.Message}");
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