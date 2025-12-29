using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Seeders;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        // 遍历枚举 RoleType
        foreach (var roleType in Enum.GetValues<RoleType>())
        {
            var roleName = roleType.ToString();

            // 如果数据库中没有该角色，则创建
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new Role
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(CultureInfo.InvariantCulture),
                    RoleType = roleType,
                    Description = $"{roleName} role" // 可选描述
                };

                await roleManager.CreateAsync(role);
            }
        }
    }
}