using Microsoft.EntityFrameworkCore;
using OrderApp.Infrastructure.Data;

namespace OrderApp.Api.Extensions;

internal static class DatabaseExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        using var scope = app.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            await dbContext.Database.MigrateAsync().ConfigureAwait(false);

            var dbName = dbContext.Database.GetDbConnection().Database;
            app.Logger.LogInformation(
                "Database '{DbName}' migrations applied successfully.", dbName);
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "Database migration failed.");
            throw;
        }
    }
}

