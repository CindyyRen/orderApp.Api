using Microsoft.EntityFrameworkCore;
using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedMenuItemsAsync(ApplicationDbContext dbContext)
    {
        if (await dbContext.MenuItems.AnyAsync())
            return; // 已经有数据就不再添加

        var items = new List<MenuItem>
        {
            new MenuItem("Chicken Rice", 1200, MenuCategory.MainCourse, "Delicious chicken with rice."),
            new MenuItem("Beef Noodle", 1500, MenuCategory.MainCourse, "Tender beef with noodles."),
            new MenuItem("Spring Roll", 500, MenuCategory.Snacks, "Crispy spring rolls.")
        };
        await dbContext.MenuItems.AddRangeAsync(items);
        await dbContext.SaveChangesAsync();
    }
}
