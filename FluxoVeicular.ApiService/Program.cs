using FluxoVeicular.ServiceDefaults.Context;
using FluxoVeicular.ServiceDefaults.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura DbContext com PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FluxoVeicularContext>(options =>
    options.UseNpgsql(connectionString));

// Registra serviço usado pelos controllers
builder.Services.AddScoped<VeiculoPlacaService>();

// Adiciona controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔥 Adiciona política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
