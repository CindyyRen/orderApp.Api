using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Data;

public sealed class DatabaseInitializer
{
    private readonly ApplicationDbContext _db;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public DatabaseInitializer(
        ApplicationDbContext db,
        RoleManager<Role> roleManager,
        UserManager<User> userManager)
    {
        _db = db;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task InitializeAsync()
    {
        await _db.Database.MigrateAsync();

        await SeedRolesAsync();
        await SeedUsersAsync();
        await SeedMenuItemsAsync();
    }

    private static async Task SeedRolesAsync() { /* 幂等 */ }
    private static async Task SeedUsersAsync() { /* 幂等 */ }
    private static async Task SeedMenuItemsAsync() { /* 幂等 */ }
}
