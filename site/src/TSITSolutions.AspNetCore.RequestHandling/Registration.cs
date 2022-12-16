using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TSITSolutions.ContactSite.RequestHandlingCore.CQRS;
using TSITSolutions.ContactSite.RequestHandlingCore.MediatRBehaviors;
using TSITSolutions.ContactSite.RequestHandlingCore.Middlewares;

namespace TSITSolutions.ContactSite.RequestHandlingCore;

public static class RegistrationExtensions
{
    public static void AddRequestHandling(this WebApplicationBuilder builder, Type queryCommandAssemblerMarkerType, Type? validationAssemblerMarkerType = null) =>
        builder.Services
            .AddMediatR(queryCommandAssemblerMarkerType)
            .AddTransient<CQRS.ISender, Sender>()
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddValidatorsFromAssembly(validationAssemblerMarkerType is not null ? validationAssemblerMarkerType.Assembly : queryCommandAssemblerMarkerType.Assembly)
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddTransient<ExceptionHandlingMiddleware>();

    public static void UseRequestHandling(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionHandlingMiddleware>();
}
