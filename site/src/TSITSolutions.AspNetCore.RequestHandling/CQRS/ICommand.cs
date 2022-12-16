using MediatR;

namespace TSITSolutions.ContactSite.RequestHandlingCore.CQRS;

public interface ICommand : IRequest<Unit> { }

public interface ICommand<out TResponse> : IRequest<TResponse> { }