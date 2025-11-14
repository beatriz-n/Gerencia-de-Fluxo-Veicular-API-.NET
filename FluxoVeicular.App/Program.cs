using FluxoVeicular.App.Client;
using FluxoVeicular.App.Client.ServiceApi;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar serviços MudBlazor globalmente
builder.Services.AddMudServices();

// Registrar Razor Components (Server + WASM interativo)
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents()
       .AddInteractiveWebAssemblyComponents();

// Registrar HttpClient para API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:4040") // URL da API
});

// Registrar serviço da API
builder.Services.AddScoped<VeiculoServiceApi>();
builder.Services.AddScoped<DashboardsServiceApi>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

// Registrar render modes
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode()
   .AddInteractiveWebAssemblyRenderMode();

app.Run();
