using FluxoVeicular.Infra.Services;
using FluxoVeicular.ServiceDefaults.Context;
using FluxoVeicular.ServiceDefaults.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FluxoVeicularContext>(options =>
    options.UseNpgsql(connectionString));

// Serviços
builder.Services.AddScoped<VeiculoPlacaService>();
builder.Services.AddScoped<DashboardService>();

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (necessário pro Blazor acessar)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Adiciona SignalR
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.MapControllers();

// 🔥 Aqui expõe o Hub
app.MapHub<NotificacaoHub>("/hub/notificacao");

app.Run();
