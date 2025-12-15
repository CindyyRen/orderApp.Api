using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Domain.Entities;

public sealed class Order
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items;

    public int TotalPrice =>
        _items.Sum(i => i.Price * i.Quantity);

    private Order() { }

    public static Order Create()
    {
        return new Order();
    }

    public void AddItem(Guid menuItemId, string name, int price, int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be > 0");

        _items.Add(new OrderItem(menuItemId, name, price, quantity));
    }

    public void RemoveItem(Guid itemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new InvalidOperationException("Item not found");

        _items.Remove(item);
    }

    public void ChangeStatus(OrderStatus status)
    {
        // 这里以后可以加状态机校验
        Status = status;
    }
}
