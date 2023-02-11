using Azure.Identity;
using FastEndpoints;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using TSITSolutions.ContactSite.Admin.Data;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
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
builder.Services.AddRazorPages();
builder.Services.AddMongoDbStore(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseFastEndpoints(x =>
    {
        x.Endpoints.RoutePrefix = "api";
        x.Errors.ResponseBuilder = (failures, _, _) => failures.Select(f => f.ErrorMessage);
    }
);

app.MapRazorPages();

app.Run();