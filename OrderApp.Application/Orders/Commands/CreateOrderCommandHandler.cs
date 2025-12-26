using MediatR;
using OrderApp.Application.Mappings;
using OrderApp.Domain.Entities;
using OrderApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Orders.Commands;

public sealed class CreateOrderCommandHandler
    //这个类负责处理 CreateOrderCommand，并且返回一个 Guid
    : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly OrderItemDtoToEntityMapper _orderItemsMapper;

    public CreateOrderCommandHandler(
        ApplicationDbContext dbContext,
        OrderItemDtoToEntityMapper orderItemsMapper)
    {
        _dbContext = dbContext;
        _orderItemsMapper = orderItemsMapper;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        ArgumentNullException.ThrowIfNull(request.Input, nameof(request.Input));

        var dto = request.Input;
        if (dto.Items.Count == 0)
            throw new InvalidOperationException("Order items cannot be empty");

        var order = Order.Create();

        // Mapper 校验并转换
        var items = await _orderItemsMapper.MapAsync(dto.Items);
        order.SetItems(items);

        // 保存订单
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}