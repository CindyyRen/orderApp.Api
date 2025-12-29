using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderApp.Api.Extensions;
using OrderApp.Api.Middleware;
using OrderApp.Application.Mappings;
using OrderApp.Application.Orders.Commands;
using OrderApp.Domain.Entities;
using OrderApp.Infrastructure;
using OrderApp.Infrastructure.Data;
using OrderApp.Infrastructure.Seeders;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<OrderItemDtoToEntityMapper>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
    // 保持属性原始 PascalCase
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
}); ;
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddFluentValidators(); // 扫描 Application 层 Validator 并注册
builder.Services.AddMediatRServices();
builder.Services.AddScoped<DatabaseInitializer>();


builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

var secret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
builder.Services.AddJwtAuthentication(secret);



var app = builder.Build();
// 全局异常处理放在最前面
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider
        .GetRequiredService<DatabaseInitializer>();

    await initializer.InitializeAsync();
}



// ✅ 添加 .ConfigureAwait(false)
await app.ApplyMigrationsAsync().ConfigureAwait(false);
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


// ✅ 添加 .ConfigureAwait(false)
await app.RunAsync().ConfigureAwait(false);