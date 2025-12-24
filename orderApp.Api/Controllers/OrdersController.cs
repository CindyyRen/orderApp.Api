using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApp.Application.DTOs.Order;
using OrderApp.Domain.Entities;
using OrderApp.Domain.ValueObjects;
using OrderApp.Infrastructure.Data;
namespace OrderApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class OrdersController(ApplicationDbContext dbContext) : ControllerBase
{
    // GET /api/orders
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAll()
    {
        var orders = await dbContext.Orders
            .Include(o => o.Items)
            .ToListAsync();

        var dtos = orders.Select(order =>
        {
            var dto = new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice.ToDecimal(), // Money → decimal
            };

            foreach (var i in order.Items)
            {
                dto.Items.Add(new OrderItemDto
                {
                    MenuItemId = i.MenuItemId,
                    Name = i.Name,
                    Price = i.Price.ToDecimal(), // Money → decimal
                    Quantity = i.Quantity
                });
            }


            return dto;
        }).ToList();


        return Ok(dtos);
    }
    // GET /api/orders
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
    {
        var order = await dbContext.Orders.Include(O => O.Items).FirstOrDefaultAsync(O => O.Id == id);
        if (order == null) return NotFound();

        var dto = new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            TotalPrice = order.TotalPrice.ToDecimal(), // Money → decimal
        };

        foreach (var i in order.Items)
        {
            dto.Items.Add(new OrderItemDto
            {
                MenuItemId = i.MenuItemId,
                Name = i.Name,
                Price = i.Price.ToDecimal(), // Money → decimal
                Quantity = i.Quantity,
            });
        }

         return Ok(dto);
    }

    // POST /api/orders
    [HttpPost]
public async Task<ActionResult<Guid>> Create([FromBody] InputOrderDto dto)
{
    ArgumentNullException.ThrowIfNull(dto);
        if (dto.Items.Count == 0)
            return BadRequest("dto.Items is empty!");


    var order = Order.Create();
    foreach (var item in dto.Items)
    {
        var menuItem = await dbContext.MenuItems.FindAsync(item.MenuItemId);
        if (menuItem == null) return BadRequest($"Menu item {item.MenuItemId} not found");
        order.AddItem(
            menuItem.Id,
            menuItem.Name,
            menuItem.Price,
            item.Quantity
        );
     }

    dbContext.Orders.Add(order);
    await dbContext.SaveChangesAsync();

        return Ok(new { orderId = order.Id,  });
    }
    // PUT /api/orders/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] InputOrderDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(updateDto);
        var order = await dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();
        var menuItemIds = updateDto.Items
            .Select(i => i.MenuItemId)
            .Distinct()
            .ToList();

        var menuItems = await dbContext.MenuItems
            .Where(m => menuItemIds.Contains(m.Id))
            .ToListAsync();
        var menuItemDict = menuItems.ToDictionary(m => m.Id);
        var domainItems = updateDto.Items.Select(dto =>
        {
            // 1️⃣ 用 MenuItemId 去字典里查
            if (!menuItemDict.TryGetValue(dto.MenuItemId, out var menuItem))
                throw new InvalidOperationException($"MenuItem {dto.MenuItemId} 不存在");

            // 2️⃣ 组装 Domain 需要的数据
            return (
                menuItem.Id,                          // 来自数据库
                menuItem.Name,
                menuItem.Price,// 来自数据库
                dto.Quantity                          // 来自前端
            );
        }).ToList();

        order.UpdateItems(domainItems);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }


    // DELETE /api/orders/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        var order = await dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
