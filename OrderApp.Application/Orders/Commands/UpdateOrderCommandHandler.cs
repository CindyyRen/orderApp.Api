using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderApp.Application.Mappings;
using OrderApp.Infrastructure.Data;
using OrderApp.Application.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Orders.Commands;

public sealed class UpdateOrderCommandHandler
    : IRequestHandler<UpdateOrderCommand, Unit>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly OrderItemDtoToEntityMapper _orderItemsMapper;

    public UpdateOrderCommandHandler(
        ApplicationDbContext dbContext,
        OrderItemDtoToEntityMapper orderItemsMapper)
    {
        _dbContext = dbContext;
        _orderItemsMapper = orderItemsMapper;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        //public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Input);

        // 查订单
        var order = await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
            throw new KeyNotFoundException("Order not found.");

        // 校验 & 映射
        var items = await _orderItemsMapper.MapAsync(request.Input.Items);
        order.SetItems(items);

        // 保存
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
