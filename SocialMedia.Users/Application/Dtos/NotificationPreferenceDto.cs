using SocialMedia.SharedKernel;
using SocialMedia.Users.Domain;
using System.Linq.Expressions;

namespace SocialMedia.Users.Application.Dtos;

internal class NotificationPreferenceDto
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public eNotificationType NotificationTypeId { get; set; }

	public bool IsMuted { get; set; }

	public static Expression<Func<NotificationPreference, NotificationPreferenceDto>> Projection => _ => new()
	{
		Id = _.Id,
		UserId = _.UserId,
		NotificationTypeId = _.NotificationTypeId,
		IsMuted = _.IsMuted,
	};
}