using FastEndpoints;
using FastEndpoints.Swagger;
using TSITSolutions.ContactSite.Server.Core;
using TSITSolutions.ContactSite.Server.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();

builder.Services.AddSingleton<IProjectRepository, InMemoryProjectRepo>();

var app = builder.Build();

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    //app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseFastEndpoints(x =>
    x.Errors.ResponseBuilder = (failures, _, _) => failures.Select(f => f.ErrorMessage)
    );

app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.Run();

public class InMemoryProjectRepo : IProjectRepository
{
    public ValueTask<IEnumerable<Project>> GetAllAsync(CancellationToken ct = default)
    {
        return ValueTask.FromResult(new []{ new Project(Guid.NewGuid(), "Test Project", "Test Description", "something", true, new DateOnly(2021,1,1), null, new []{"C#", "Azure"}) }.AsEnumerable());
    }

    public ValueTask<Project> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return ValueTask.FromResult(new Project(Guid.NewGuid(), "Test Project", "Test Description", "something", true,
            new DateOnly(2021, 1, 1), null, new[] { "C#", "Azure" }));
    }
}