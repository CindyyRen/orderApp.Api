using Humanizer;
using Microsoft.EntityFrameworkCore;
using OrderApp.Application.DTOs.Order;
using OrderApp.Domain.ValueObjects;
using OrderApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Mappings;

public class OrderItemDtoToEntityMapper
{
    private readonly ApplicationDbContext _dbContext;

    public OrderItemDtoToEntityMapper(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<(Guid menuItemId, string name, Money price, int quantity)>>
        MapAsync(IEnumerable<InputOrderItemDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);

        var dtoList = dtos.ToList();
        if (dtoList.Count == 0)
            throw new InvalidOperationException("Order items cannot be empty");

        var menuItemIds = dtoList
            .Select(i => i.MenuItemId)
            .Distinct()
            .ToList();

        var menuItems = await _dbContext.MenuItems
            .Where(m => menuItemIds.Contains(m.Id))
            .ToDictionaryAsync(m => m.Id);

        if (menuItems.Count != menuItemIds.Count)
            throw new InvalidOperationException("Some menu items not found");

        return dtoList.Select(i =>
        {
            var menuItem = menuItems[i.MenuItemId];
            return (menuItem.Id, menuItem.Name, menuItem.Price, i.Quantity);
        }).ToList();
    }
}
