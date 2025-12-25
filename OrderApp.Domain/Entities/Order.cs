using OrderApp.Domain.Common;
using OrderApp.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Domain.Entities;

public sealed class Order:BaseEntity
{
    //public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid CustomerId { get; private set; }

    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public List<OrderItem> Items { get; private set; } = new();


    // ✅ 修复：返回 Money 类型
    public Money TotalPrice =>
    Items.Count == 0
        ? Money.Zero
        : Items
            .Select(i => i.Price * i.Quantity)   // 需要 Money * int
            .Aggregate((sum, price) => sum + price);  // Money + Money

    private Order()
    {
        // Id 已经在 BaseEntity 中自动生成了
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public static Order Create()
    {
        return new Order();
    }
    // ✅ 私有方法：检查是否可以修改菜品
    private void EnsureCanModifyItems()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"订单状态为 {Status}，不能修改菜品");
    }
    // ✅ 私有方法：更新时间
    private void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    // 🔹 检查是否可以取消
    public bool CanCancel()
    {
        return Status == OrderStatus.Pending || Status == OrderStatus.Ready;
    }

    // ✅ 唯一的 items 操作方法 - 适用于 Create 和 Update
    public void SetItems(IEnumerable<(Guid menuItemId, string name, Money price, int quantity)> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        EnsureCanModifyItems();

        Items.Clear();  // 对于 Create 来说，清空空列表 = 无操作

        foreach (var (menuItemId, name, price, quantity) in items)
        {
            Items.Add(new OrderItem(this, menuItemId, name, price, quantity));
        }

        Touch();
    }
    public void ChangeStatus(OrderStatus status)
    {
        // 这里以后可以加状态机校验
        Status = status;
    }
}
