using MediatR;
using Microsoft.Extensions.Logging;

namespace TSITSolutions.ContactSite.RequestHandlingCore.MediatRBehaviors;

internal class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest: class, IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {RequestName}", typeof(TRequest).Name);
        var response = await next();
        _logger.LogDebug("Handled {RequestName}", typeof(TRequest).Name);

        return response;
    }
}