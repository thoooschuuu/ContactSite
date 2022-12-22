using FastEndpoints;
using FastEndpoints.Swagger;
using TSITSolutions.ContactSite.Server.MongoDb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();

builder.Services.AddMongoDbStore(builder.Configuration);

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
