using MediatR;

namespace TSITSolutions.ContactSite.RequestHandlingCore.CQRS;

public interface ISender
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new());

    Task<object?> Send(object request, CancellationToken cancellationToken = new());
}
