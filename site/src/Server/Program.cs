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
    private readonly Project[] _projects = {
        new(Guid.Parse("EBFF4563-6354-40CB-A5C4-B898593CB09D"), "Software Architect", "Test Description", "I've done things", "something", new DateOnly(2021,1,1), null, new []{"C#", "Azure", "React", "Terraform", "Docker", "AWS", "Azure DevOps", "Kanban", "WSJF"}),
        new(Guid.Parse("F63687D3-107D-4E07-92D2-AD1652FE6CFF"), "Technical Lead", "Test Description", "I've done things", "something", new DateOnly(2021,1,1), new DateOnly(2020,12,31), new []{"C#", "Azure"}),
        new(Guid.Parse("50332769-AE11-466C-AF2A-9EABD01C4865"), "Developer", "Test Description", "I've done things", "something", new DateOnly(2018,4,1), new DateOnly(2019,12,31), new []{"C#", "Azure"}),
        new(Guid.Parse("74235B74-8A16-427B-8437-71F7928C241E"), "Consultant", "Test Description", "I've done things", "something", new DateOnly(2016,7,1), new DateOnly(2018,3,31), new []{"C#", "Azure"}),
        new(Guid.Parse("90E2EC27-4E7D-4511-A848-E6990DF3E93E"), "Junior Consultant", "Test Description", "I've done things", "something", new DateOnly(2015,7,1), new DateOnly(2016,6,30), new []{"C#", "TFS"}),
        new(Guid.Parse("23FB9C49-3179-4A8F-8AFD-EEAC8C80977E"), "Student", "Test Description", "I've done things", "something", new DateOnly(2010,10,31), new DateOnly(2015,6,30), new []{"C#", "TFS"}),
    };
    public ValueTask<IEnumerable<Project>> GetAllAsync(CancellationToken ct = default)
    {
        return ValueTask.FromResult(_projects.AsEnumerable());
    }

    public ValueTask<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return ValueTask.FromResult(_projects.FirstOrDefault(p => p.Id == id));
    }
}