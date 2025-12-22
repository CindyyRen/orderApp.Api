using Microsoft.EntityFrameworkCore;
using OrderApp.Api.Extensions;
using OrderApp.Infrastructure;
using OrderApp.Infrastructure.Data;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
    // 保持属性原始 PascalCase
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
}); ;
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbSeeder.SeedMenuItemsAsync(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // 删除并重建数据库
    //db.Database.EnsureDeleted();
    db.Database.Migrate();  // 先应用迁移,创建表结构

    // 然后才调用种子数据
    await DbSeeder.SeedMenuItemsAsync(db);
}

// ✅ 添加 .ConfigureAwait(false)
await app.ApplyMigrationsAsync().ConfigureAwait(false);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


// ✅ 添加 .ConfigureAwait(false)
await app.RunAsync().ConfigureAwait(false);