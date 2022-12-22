using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TSITSolutions.ContactSite.Client;
using TSITSolutions.ContactSite.Client.Extensions;
using TSITSolutions.ContactSite.Client.Infrastructure;
using TSITSolutions.ContactSite.Client.Shared.Resources;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<StronglyTypedStringLocalizerForResource>();
builder.Services.AddSingleton<TechnologyService>();

builder.Services.AddMudServices();

builder.Services.AddLocalization();
builder.Services.AddBlazoredLocalStorage();

var host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();
