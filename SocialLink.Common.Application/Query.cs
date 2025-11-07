using MediatR;
using SocialLink.Common.Data;
using SocialLink.SharedKernel;

namespace SocialLink.Common.Application;

public record Query<TResponse> : IRequest<ResponseWrapper<TResponse>>;

public abstract class EFQueryHandler<TQuery, TResponse>(IEFDatabaseContext db) : EFUseCase(db), IRequestHandler<TQuery, ResponseWrapper<TResponse>>
	where TQuery : Query<TResponse>
{
	public abstract Task<ResponseWrapper<TResponse>> Handle(TQuery req, CancellationToken ct);
}

public abstract class MongoQueryHandler<TQuery, TResponse> : MongoUseCase, IRequestHandler<TQuery, ResponseWrapper<TResponse>>
	where TQuery : Query<TResponse>
{
	public abstract Task<ResponseWrapper<TResponse>> Handle(TQuery req, CancellationToken ct);
}