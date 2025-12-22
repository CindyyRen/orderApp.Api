using OrderApp.Domain.Common;
using OrderApp.Domain.ValueObjects;

namespace OrderApp.Domain.Entities;
//核心原则：如果一个方法名包含业务术语（discount, tax, tip, service charge），就不应该放在 Money 类中。
public class MenuItem:BaseEntity
{
    //没有初始化 → 默认值是 0（因为 int 默认就是 0）。
    //public Guid Id { get; private set; }

    //public int Id { get; private set; } // Aggregate Root
    public string Name { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
    public bool IsAvailable { get; private set; } = true;
    public string? Description { get; private set; }
    public MenuCategory Category { get; private set; }
    // ✅ 添加审计字段
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    //没写任何构造函数 → C# 默认生成 public 无参构造函数 → EF Core 可以用
    //写了带参数构造函数 → 默认无参构造函数消失 → EF Core 会报错
    //解决方法 → 手动加一个无参构造函数，private 就行
    private MenuItem() { } // EF Core only

    public MenuItem(
        string name,
        Money price,
        MenuCategory category,
        string? description)
    {
        Name = name;
        Price = price;
        Category = category;
        Description = description;
        IsAvailable = true;
        CreatedAt = DateTime.UtcNow;
    }


    // ✅ 唯一对外创建入口
    public static MenuItem Create(
        string name,
        Money price,
        MenuCategory category,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        ArgumentNullException.ThrowIfNull(price);

        return new MenuItem(name, price, category, description);
    }
    // 行为方法
    public void UpdatePrice(Money newPrice)
    {
        ArgumentNullException.ThrowIfNull(newPrice);
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Name cannot be empty");
        Name = newName;
    }

    public void MarkAsAvailable() => IsAvailable = true;
    public void MarkAsUnavailable() => IsAvailable = false;

    public void UpdateDescription(string? description) => Description = description;


}
