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


var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // habilita CORS

app.MapControllers();

app.Run();
