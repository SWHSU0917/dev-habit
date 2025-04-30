using DevHabit.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Extensions;

/// <summary>
/// 提供資料庫擴充方法，用於在應用啟動時自動套用資料庫遷移。
/// </summary>
public static class DatabaseExtensions
{

    /// <summary>
    /// 擴充方法：在 ASP.NET Core 應用啟動時，自動執行 EF Core 的遷移操作。
    /// </summary>
    /// <param name="app">WebApplication 執行個體。</param>
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        // 建立服務範圍（Scope），以便從 DI 容器中取得必要服務
        using IServiceScope scope = app.Services.CreateScope();

        // 從 DI 容器中解析出 ApplicationDbContext
        await using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            // 非同步套用所有尚未執行的資料庫遷移
            await dbContext.Database.MigrateAsync();

            app.Logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "An error occurred while applying database migrations.");
            throw;
        }
    }
}
