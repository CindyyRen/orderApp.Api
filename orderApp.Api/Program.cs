using Microsoft.EntityFrameworkCore;
using OrderApp.Api.Extensions;
using OrderApp.Infrastructure;
using OrderApp.Infrastructure.Data;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
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
}
// ✅ 添加 .ConfigureAwait(false)
await app.ApplyMigrationsAsync().ConfigureAwait(false);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


// ✅ 添加 .ConfigureAwait(false)
await app.RunAsync().ConfigureAwait(false);