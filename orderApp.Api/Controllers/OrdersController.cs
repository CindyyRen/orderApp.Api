using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApp.Application.DTOs.Order;
using OrderApp.Domain.Entities;
using OrderApp.Domain.ValueObjects;
using OrderApp.Infrastructure.Data;
using OrderApp.Application.Mappings;

namespace OrderApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class OrdersController(ApplicationDbContext dbContext, OrderItemDtoToEntityMapper _orderItemsMapper) : ControllerBase
{
    // GET /api/orders
    // GET /api/orders
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAll()
    {
        var orders = await dbContext.Orders
            .Include(o => o.Items)
            .ToListAsync();

        // 使用你写好的扩展方法映射
        var dtos = orders.Select(o => o.ToDto()).ToList();

        return Ok(dtos);
    }
    // GET /api/orders
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
    {
        var order = await dbContext.Orders.Include(O => O.Items).FirstOrDefaultAsync(O => O.Id == id);
        if (order == null) return NotFound();
        var dto = order.ToDto();
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

        try
        {
            var items = await _orderItemsMapper.MapAsync(dto.Items);
            order.SetItems(items);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();

        return Ok(new { orderId = order.Id });

    }
    // PUT /api/orders/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] InputOrderDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(updateDto);

        var order = await dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound();

        try
        {
            var items = await _orderItemsMapper.MapAsync(updateDto.Items);
            order.SetItems(items);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }

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
