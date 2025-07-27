using Ardalis.Result;
using MediatR;

namespace SocialMedia.SharedKernel.UseCases;

public record Query<TResponse> : IRequest<Result<TResponse>>;

public abstract class EFQueryHandler<TQuery, TResponse>(IEFDatabaseContext db) : EFUseCase(db), IRequestHandler<TQuery, Result<TResponse>>
	where TQuery : Query<TResponse>
{
	public abstract Task<Result<TResponse>> Handle(TQuery req, CancellationToken ct);
}

public abstract class MongoQueryHandler<TQuery, TResponse> : MongoUseCase, IRequestHandler<TQuery, Result<TResponse>>
	where TQuery : Query<TResponse>
{
	public abstract Task<Result<TResponse>> Handle(TQuery req, CancellationToken ct);
}