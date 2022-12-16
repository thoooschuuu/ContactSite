using MediatR;

namespace TSITSolutions.ContactSite.RequestHandlingCore.CQRS;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse> 
    where TQuery : IQuery<TResponse> { }