using Ardalis.Result;
using MediatR;

namespace SocialMedia.SharedKernel.UseCases;

public record Query<TResponse> : IRequest<Result<TResponse>>;

public abstract class QueryHandler<TQuery, TResponse>(IAppDatabaseContext db) : UseCase(db), IRequestHandler<TQuery, Result<TResponse>>
	where TQuery : Query<TResponse>
{
	public abstract Task<Result<TResponse>> Handle(TQuery req, CancellationToken ct);
}