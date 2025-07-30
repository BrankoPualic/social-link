using SocialMedia.Posts.Application.Dtos;

namespace SocialMedia.Posts.Endpoints;

internal sealed record CreatePostRequest(PostEditDto Model);