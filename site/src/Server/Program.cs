using Azure.Identity;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using TSITSolutions.ContactSite.Server.Caching;
using TSITSolutions.ContactSite.Server.MongoDb;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    Console.WriteLine("Not development environment");
    builder.Configuration.AddAzureAppConfiguration(configBuilder =>
    {
        var connectionString = builder.Configuration.GetConnectionString("AppConfig");
        var credential = new DefaultAzureCredential();
        configBuilder.Connect(new Uri(connectionString), credential)
            .ConfigureKeyVault(kv => kv.SetCredential(credential))
            .Select(KeyFilter.Any, "contact-site")
            .ConfigureRefresh(refresh =>
            {
                refresh.Register("contact-site:Cache:Refresh", refreshAll: true)
                    .SetCacheExpiration(TimeSpan.FromMinutes(30));
            });
    });
}

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();

builder.Services.Configure<CachingSettings>(builder.Configuration.GetSection("Caching"));
builder.Services.AddResponseCaching(options =>
{
    options.UseCaseSensitivePaths = false;
});

builder.Services.AddMongoDbStore(builder.Configuration);

var app = builder.Build();

app.InitializeMongoDbStore();

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
if(!app.Environment.IsDevelopment())
{
    app.UseResponseCaching();
}

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
