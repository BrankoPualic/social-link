using FastEndpoints;
using FluentValidation;
using SocialMedia.Posts.Application.Dtos;
using SocialMedia.Posts.Domain;
using SocialMedia.SharedKernel;

namespace SocialMedia.Posts.Endpoints;

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