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

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items;

    // ✅ 修复：返回 Money 类型
    public Money TotalPrice =>
    _items.Count == 0
        ? Money.Zero
        : _items
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
    // ✅ 添加订单项
    public void AddItem(Guid menuItemId, string name, Money price, int quantity)
    {
        // 1️⃣ 确保订单状态允许修改
        EnsureCanModifyItems();

        // 2️⃣ 处理重复菜品：累加数量或新增
        var existingItem = _items.FirstOrDefault(x => x.MenuItemId == menuItemId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            _items.Add(new OrderItem(this, menuItemId, name, price, quantity));
        }

        // 3️⃣ 更新时间
        Touch();
    }
    // ✅ 更新订单
    public void UpdateItems(IReadOnlyCollection<(Guid menuItemId, string name, Money price, int quantity)> newItems)
    {
        // ✅ 简洁 null 检查
    ArgumentNullException.ThrowIfNull(newItems);

        // 1️⃣ 确保可以修改
        EnsureCanModifyItems();

        // 2️⃣ 清空旧数据
        _items.Clear();

        // 3️⃣ 添加新数据（直接add，不要用AddItem，因为AddItem有累加逻辑）
        foreach (var (menuItemId, name, price, quantity) in newItems)
        {
            _items.Add(new OrderItem(this, menuItemId, name, price, quantity));
        }

        // 4️⃣ 更新时间
        Touch();
    }

    public void ChangeStatus(OrderStatus status)
    {
        // 这里以后可以加状态机校验
        Status = status;
    }
}
