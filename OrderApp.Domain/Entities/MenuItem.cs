namespace OrderApp.Domain.Entities;
//核心原则：如果一个方法名包含业务术语（discount, tax, tip, service charge），就不应该放在 Money 类中。
public class MenuItem
{
    //没有初始化 → 默认值是 0（因为 int 默认就是 0）。
    public Guid Id { get; private set; }

    //public int Id { get; private set; } // Aggregate Root
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public bool IsAvailable { get; private set; } = true;
    public string? Description { get; private set; }
    public MenuCategory Category { get; private set; }
    // ✅ 添加审计字段
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    //没写任何构造函数 → C# 默认生成 public 无参构造函数 → EF Core 可以用
    //写了带参数构造函数 → 默认无参构造函数消失 → EF Core 会报错
    //解决方法 → 手动加一个无参构造函数，private 就行
    private MenuItem() { } // For EF Core

    // 构造函数，保证初始数据合法
    public MenuItem(string name, decimal price, MenuCategory category, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty");
        if (price < 0) throw new ArgumentException("Price cannot be negative");

        Name = name;
        Price = price;
        Category = category;
        Description = description;
        IsAvailable = true;
    }

    // 行为方法
    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0) throw new ArgumentException("Price cannot be negative");
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow; // ✅ 记录更新时间
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
