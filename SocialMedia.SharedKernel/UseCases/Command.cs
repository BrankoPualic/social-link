using Ardalis.Result;
using MediatR;

namespace SocialMedia.SharedKernel.UseCases;

public record Command : IRequest<Result>;

public record Command<TResponse> : IRequest<Result<TResponse>>;

public abstract class EFCommandHandler<TCommand>(IEFDatabaseContext db) : EFUseCase(db), IRequestHandler<TCommand, Result>
	where TCommand : Command
{
	public abstract Task<Result> Handle(TCommand req, CancellationToken ct);
}

public abstract class EFCommandHandler<TCommand, TResponse>(IEFDatabaseContext db) : EFUseCase(db), IRequestHandler<TCommand, Result<TResponse>>
	where TCommand : Command<TResponse>
{
	public abstract Task<Result<TResponse>> Handle(TCommand req, CancellationToken ct);
}

public abstract class MongoCommandHandler<TCommand> : MongoUseCase, IRequestHandler<TCommand, Result>
	where TCommand : Command
{
	public abstract Task<Result> Handle(TCommand req, CancellationToken ct);
}

public abstract class MongoCommandHandler<TCommand, TResponse> : MongoUseCase, IRequestHandler<TCommand, Result<TResponse>>
	where TCommand : Command<TResponse>
{
	public abstract Task<Result<TResponse>> Handle(TCommand req, CancellationToken ct);
}