using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Domain.Entities;

public sealed class OrderItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid MenuItemId { get; private set; }
    public string Name { get; private set; } = null!;
    public int Price { get; private set; }
    public int Quantity { get; private set; }

    private OrderItem() { }

    internal OrderItem(Guid menuItemId, string name, int price, int quantity)
    {
        MenuItemId = menuItemId;
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}

