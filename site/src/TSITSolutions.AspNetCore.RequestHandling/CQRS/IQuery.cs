using MediatR;

namespace TSITSolutions.ContactSite.RequestHandlingCore.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse> { }