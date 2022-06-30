using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace TSITSolutions.ContactSite.Client.Extensions;

public static class WebAssemblyHostExtensions
{
    public static async ValueTask SetDefaultCulture(this WebAssemblyHost host)
    {
        var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
        var cultureString = await localStorage.GetItemAsync<string>("culture");

        var cultureInfo = !string.IsNullOrWhiteSpace(cultureString) 
            ? new CultureInfo(cultureString) 
            : new CultureInfo("de-DE");

        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }
}