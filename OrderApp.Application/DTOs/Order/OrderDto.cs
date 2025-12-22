using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.DTOs.Order;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    // ✅ 改为 decimal，在 API 层展示
    public decimal TotalPrice { get; set; }
    //private readonly List<OrderItemDto> _items = new();
    //public ReadOnlyCollection<OrderItemDto> Items => _items.AsReadOnly();
    public Collection<OrderItemDto> Items { get; } = new();
}
