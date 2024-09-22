using CreditTransactionApi.Data;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var connectionString = ConnectionString.GenerateFromEnvironment();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString, c => c.MigrationsAssembly("Data"))
);

builder.Services.AddSingleton<TransactionRequestValidator>();

var app = builder.Build();

app.UseHealthChecks("/healthcheck");

app.Logger.LogInformation("Start: database migration");
using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
context.Database.Migrate();
app.Logger.LogInformation("Finish: database migration");

app.MapPost("/transaction", (TransactionRequestPayload payload, TransactionRequestValidator payloadValidator) =>
{
    var validationResult = payloadValidator.Validate(payload);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }

    return Results.Ok($"Created /transaction/{payload.account}");
});

app.Run();