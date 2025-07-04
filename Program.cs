using Microsoft.EntityFrameworkCore;
using botAPI.Data;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



// Registre seu serviço
var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Aqui que pode estar o erro se o banco não conecta
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoSomee")));

// Aqui que pode estar o erro se o banco não conecta
builder.Services.AddDbContext<MLDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MLDatabase")));


AppContext.SetSwitch("System.Transactions.TransactionManager.ImplicitDistributedTransactions", true);



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
