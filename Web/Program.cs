using CreditTransactionApi.Data;
using CreditTransactionApi.Web;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var connectionString = ConnectionString.GenerateFromEnvironment();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString, c => c.MigrationsAssembly("Data"))
);

builder.AddDepencyInjection();

var app = builder.Build();

app.UseHealthChecks("/healthcheck");
app.UseMigration();

app.UseTransactionEndpoints();
app.UseOutOfScopeEndpoints();

app.Run();