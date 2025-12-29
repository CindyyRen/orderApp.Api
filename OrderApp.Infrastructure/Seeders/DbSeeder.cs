using Microsoft.EntityFrameworkCore;
using OrderApp.Domain.Entities;
using OrderApp.Domain.ValueObjects;
using OrderApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Seeders;

public static class DbSeeder
{
    public static async Task SeedMenuItemsAsync(ApplicationDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        // 检查是否已有数据
        if (await dbContext.MenuItems.AnyAsync())
        {
            return; // 已有数据,跳过
        }

        // 添加种子数据
        var items = new List<MenuItem>
        {
            MenuItem.Create("Chicken Rice", Money.FromCents(1200), MenuCategory.MainCourse, "Delicious chicken with rice."),
            MenuItem.Create("Beef Noodle", Money.FromCents(1500), MenuCategory.MainCourse, "Tender beef with noodles."),
            MenuItem.Create("Spring Roll", Money.FromCents(500), MenuCategory.Snacks, "Crispy spring rolls.")
        };

        try
        {
            await dbContext.MenuItems.AddRangeAsync(items);
            await dbContext.SaveChangesAsync();
            Console.WriteLine($"成功添加 {items.Count} 条菜单数据");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"种子数据添加失败: {ex.InnerException?.Message ?? ex.Message}");
            throw;
        }
    }
}