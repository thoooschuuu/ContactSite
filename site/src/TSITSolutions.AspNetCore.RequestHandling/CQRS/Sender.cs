using MediatR;

namespace TSITSolutions.ContactSite.RequestHandlingCore.CQRS;

public class Sender : ISender
{
    private readonly MediatR.ISender _sender;

    public Sender(MediatR.ISender sender)
    {
        _sender = sender;
    }
    
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new()) =>
        _sender.Send(request, cancellationToken);
    
    public Task<object?> Send(object request, CancellationToken cancellationToken = new()) =>
        _sender.Send(request, cancellationToken);
}