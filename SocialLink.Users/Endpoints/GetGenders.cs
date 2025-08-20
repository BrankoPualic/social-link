using FastEndpoints;
using SocialLink.SharedKernel;

namespace SocialLink.Users.Endpoints;

internal class GetGenders : Endpoint<EmptyRequest, List<LookupDto>>
{
	public override void Configure()
	{
		Get("/users/genders");
		AllowAnonymous();
	}

	public override Task HandleAsync(EmptyRequest req, CancellationToken ct)
	{
		var result = Enum.GetValues<eGender>()
			.Select(LookupDto.EnumProjection)
			.ToList();

		return Send.OkAsync(result, ct);
	}
}