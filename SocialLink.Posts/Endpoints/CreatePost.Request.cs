using SocialLink.Posts.Application.Dtos;

namespace SocialLink.Posts.Endpoints;

internal sealed record CreatePostRequest(PostEditDto Model);