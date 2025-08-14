using FastEndpoints;
using FluentValidation;
using SocialLink.Posts.Application.Dtos;
using SocialLink.Posts.Domain;
using SocialLink.SharedKernel;

namespace SocialLink.Posts.Endpoints;

internal class UpdatePostArchiveStatusValidator : Validator<PostEditDto>
{
	public UpdatePostArchiveStatusValidator()
	{
		RuleFor(_ => _.Id)
			.NotNull()
			.NotEmpty()
			.WithMessage(ResourcesValidation.Required(nameof(Post.Id)));
	}
}