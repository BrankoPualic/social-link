using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SocialLink.SharedKernel.Extensions;

namespace SocialLink.Messaging.Application.ScheduledTasks;
internal class EmptyChatGroupsCleanupScheduledTask(IServiceProvider serviceProvider, ILogger logger) : IHostedService, IDisposable
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
		var db = scope.ServiceProvider.GetRequiredService<IEFMessagingDatabaseContext>();

		var deadline = DateTime.UtcNow.AddDays(-1);
		try
		{
			var chatGroups = db.ChatGroups
				.Where(_ => _.LastMessageOn == null)
				.Where(_ => _.CreatedOn < deadline)
				.ToList();

			if (chatGroups.Count < 1)
			{
				logger.Information("No empty chat group deleted.");
				return;
			}

			var chatGroupIds = chatGroups.SelectIds(_ => _.Id);
			var members = db.ChatGroupUsers
				.Where(_ => chatGroupIds.Contains(_.ChatGroupId))
				.ToList();

			var media = db.Media
				.Where(_ => chatGroupIds.Contains(_.ChatGroupId))
				.ToList();

			db.ChatGroupUsers.RemoveRange(members);
			db.Media.RemoveRange(media);
			db.ChatGroups.RemoveRange(chatGroups);

			db.SaveChanges();
		}
		catch (Exception ex)
		{
			logger.Error(ex, $"EXCEPTION: Empty Chat Groups Cleanup Schedule Task\n----------\n{ex.Message}");
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
