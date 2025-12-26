using OrderApp.Application.DTOs.Order;
using OrderApp.Domain.Entities;
using OrderApp.Application.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Mappings;

public static class OrderToDtoMapper
{
    public static OrderDto ToDto(this Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        var dto = new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            TotalPrice = order.TotalPrice.ToDecimal()
        };

        foreach (var item in order.Items)
        {
            dto.Items.Add(new OrderItemDto
            {
                MenuItemId = item.MenuItemId,
                Name = item.Name,
                Price = item.Price.ToDecimal(),
                Quantity = item.Quantity
            });
        }

        return dto;
    }
}
