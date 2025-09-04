using System.Net.Http;
using FluxoVeicular.App.Client;
using FluxoVeicular.Web.ServiceApi;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton(sp =>
{
    var connection = new HubConnectionBuilder()
        .WithUrl("https://localhost:4040/hub/notificacao")
        .WithAutomaticReconnect()
        .Build();

    return connection;
});

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
