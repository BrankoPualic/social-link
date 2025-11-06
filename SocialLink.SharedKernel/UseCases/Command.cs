using MediatR;

namespace SocialLink.SharedKernel.UseCases;

public record Command : IRequest<ResponseWrapper>;

public record Command<TResponse> : IRequest<ResponseWrapper<TResponse>>;

public abstract class EFCommandHandler<TCommand>(IEFDatabaseContext db) : EFUseCase(db), IRequestHandler<TCommand, ResponseWrapper>
	where TCommand : Command
{
	public abstract Task<ResponseWrapper> Handle(TCommand req, CancellationToken ct);
}

public abstract class EFCommandHandler<TCommand, TResponse>(IEFDatabaseContext db) : EFUseCase(db), IRequestHandler<TCommand, ResponseWrapper<TResponse>>
	where TCommand : Command<TResponse>
{
	public abstract Task<ResponseWrapper<TResponse>> Handle(TCommand req, CancellationToken ct);
}

public abstract class MongoCommandHandler<TCommand> : MongoUseCase, IRequestHandler<TCommand, ResponseWrapper>
	where TCommand : Command
{
	public abstract Task<ResponseWrapper> Handle(TCommand req, CancellationToken ct);
}

public abstract class MongoCommandHandler<TCommand, TResponse> : MongoUseCase, IRequestHandler<TCommand, ResponseWrapper<TResponse>>
	where TCommand : Command<TResponse>
{
	public abstract Task<ResponseWrapper<TResponse>> Handle(TCommand req, CancellationToken ct);
}