using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApp.Application.DTOs.Order;
using OrderApp.Application.Mappings;
using OrderApp.Application.Orders.Commands;
using OrderApp.Application.Orders.Queries;
using OrderApp.Domain.Entities;
using OrderApp.Domain.ValueObjects;
using OrderApp.Infrastructure.Data;

namespace OrderApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class OrdersController(IMediator mediator) : ControllerBase
{
    // GET /api/orders
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAll()
        => Ok(await mediator.Send(new GetAllOrdersQuery()));

    // GET /api/orders/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
        => Ok(await mediator.Send(new GetOrderByIdQuery(id)));

    // POST /api/orders
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] InputOrderDto dto)
        => Ok(new { orderId = await mediator.Send(new CreateOrderCommand(dto)) });

    // PUT /api/orders/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] InputOrderDto updateDto)
    {
        await mediator.Send(new UpdateOrderCommand(id, updateDto));
        return NoContent();
    }

    // DELETE /api/orders/{id}
    [HttpDelete("{id}")]
    //[Authorize(Roles = "Manager,Cashier")] // 如果删除要权限，可以加 Roles
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        await mediator.Send(new DeleteOrderCommand(id));
        return NoContent();
    }
}
