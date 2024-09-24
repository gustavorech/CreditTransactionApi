using CreditTransactionApi.Data;

using Microsoft.EntityFrameworkCore;

namespace CreditTransactionApi.Web;

public static class Migration
{
    public static void UseMigration(this WebApplication app)
    {
        app.Logger.LogInformation("Start: database migration");
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        context.Database.Migrate();
        app.Logger.LogInformation("Finish: database migration");
    }
}