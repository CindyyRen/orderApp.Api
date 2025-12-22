using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.DTOs.Order;

public class OrderItemDto
{
    public Guid MenuItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    // ✅ 改为 decimal
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
