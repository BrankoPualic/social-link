using Ardalis.Result;
using MediatR;

namespace SocialMedia.SharedKernel.UseCases;

public record Command : IRequest<Result>;

public record Command<TResponse> : IRequest<Result<TResponse>>;

public abstract class CommandHandler<TCommand>(IAppDatabaseContext db) : UseCase(db), IRequestHandler<TCommand, Result>
	where TCommand : Command
{
	public abstract Task<Result> Handle(TCommand req, CancellationToken ct);
}

public abstract class CommandHandler<TCommand, TResponse>(IAppDatabaseContext db) : UseCase(db), IRequestHandler<TCommand, Result<TResponse>>
	where TCommand : Command<TResponse>
{
	public abstract Task<Result<TResponse>> Handle(TCommand req, CancellationToken ct);
}