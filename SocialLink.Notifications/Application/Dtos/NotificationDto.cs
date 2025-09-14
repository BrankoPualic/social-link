using SocialLink.Notifications.Domain;
using SocialLink.SharedKernel;
using System.Linq.Expressions;

namespace SocialLink.Notifications.Application.Dtos;

internal class NotificationDto
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public eNotificationType TypeId { get; set; }

	public string Title { get; set; }

	public string Message { get; set; }

	public string Details { get; set; }

	public bool IsRead { get; set; }

	public DateTime CreatedOn { get; set; }

	public static Expression<Func<Notification, NotificationDto>> Projection => _ => new()
	{
		Id = _.Id,
		UserId = _.UserId,
		TypeId = _.TypeId,
		Title = _.Title,
		Message = _.Message,
		Details = _.Details,
		IsRead = _.IsRead,
		CreatedOn = _.CreatedOn,
	};
}