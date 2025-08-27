using FluxoVeicular.App.Client;
using FluxoVeicular.Web.ServiceApi;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root Component
//builder.RootComponents.Add<App>("#app");

// HttpClient para API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:4040") // URL da sua API
});

// Registrar serviços
builder.Services.AddScoped<VeiculoServiceApi>();

// MudBlazor
builder.Services.AddMudServices();

await builder.Build().RunAsync();
