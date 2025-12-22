using OrderApp.Domain.Common;
using OrderApp.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrderApp.Domain.Entities;

public sealed class OrderItem:BaseEntity
{
    //public Guid Id { get; private set; } = Guid.NewGuid();
    //推荐做法：在 OrderItem 中存一份 Name（和 Price）快照，保持订单历史记录稳定
    //MenuItem 改名字不会影响已生成订单
    // _items 的 Price / Name 就是用户下单时的状态
    public Guid MenuItemId { get; private set; }
    public string Name { get; set; } = string.Empty;
    public Money Price { get; set; } = Money.Zero;

    public int Quantity { get; private set; }
    // ✅ 显式加上 OrderId
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;
    private OrderItem() { } // 仅供 EF Core 使用

    public OrderItem(Order order,  Guid menuItemId,string name,Money price,
    int quantity)
    {
        ArgumentNullException.ThrowIfNull(price);
        ArgumentNullException.ThrowIfNull(order);
        if (menuItemId == Guid.Empty) throw new ArgumentException("MenuItemId cannot be empty");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty");
        if (quantity <= 0) throw new InvalidOperationException("Quantity must be > 0");
        Order = order;        // ✅ 这一行是“关系成立”的关键
        OrderId = order.Id;  // 🔥 关键！显式设置外键
        MenuItemId = menuItemId;
        Name = name;
        Price = price;
        Quantity = quantity;
    }
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than 0");

        Quantity = newQuantity;
    }

}

