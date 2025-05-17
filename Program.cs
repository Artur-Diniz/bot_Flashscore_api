using Microsoft.EntityFrameworkCore;
using botAPI.Data;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL;


var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Aqui que pode estar o erro se o banco não conecta
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoSomee")));

builder.Services.AddDbContext<MLDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MLDatabase")));

// Registrar o serviço de sincronização
builder.Services.AddScoped<MLDataSyncService>();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
