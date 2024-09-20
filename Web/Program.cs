using CreditTransactionApi.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var connectionString = ConnectionString.GenerateFromEnvironment();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString, c => c.MigrationsAssembly("Data"))
);

var app = builder.Build();

app.UseHealthChecks("/healthcheck");

app.Run();