using SocialLink.SharedKernel.Enumerators;
using SocialLink.SharedKernel.Extensions;
using SocialLink.Users.Domain;

namespace SocialLink.Users.Application.Dtos;

internal class NotificationPreferenceDto
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public eNotificationType NotificationTypeId { get; set; }

	public string Name { get; set; }

	public bool IsMuted { get; set; }

	public static Func<NotificationPreference, NotificationPreferenceDto> InMemoryProjection => _ => new()
	{
		Id = _.Id,
		UserId = _.UserId,
		NotificationTypeId = _.NotificationTypeId,
		Name = _.NotificationTypeId.GetDescription(),
		IsMuted = _.IsMuted,
	};

	public void ToModel(NotificationPreference model)
	{
		model.UserId = UserId;
		model.NotificationTypeId = NotificationTypeId;
		model.IsMuted = IsMuted;
	}
}