using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Seeders;

public static class UserSeeder
{
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        // 默认用户列表
        var users = new[]
        {
            new { UserName = "admin", Email = "admin@test.com", Role = RoleType.Admin },
            new { UserName = "manager", Email = "manager@test.com", Role = RoleType.Manager },
            new { UserName = "user", Email = "user@test.com", Role = RoleType.User }
        };

        foreach (var u in users)
        {
            var user = await userManager.FindByNameAsync(u.UserName);
            if (user == null)
            {
                user = new User
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    FullName = u.UserName,
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                // 默认密码
                await userManager.CreateAsync(user, "P@ssw0rd!");
            }

            // 给用户分配角色
            var roleName = u.Role.ToString();
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
