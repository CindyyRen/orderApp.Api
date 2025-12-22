using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.DTOs.Order;

public sealed class InputOrderItemDto
{
    public Guid MenuItemId { get; set; }
    public int Quantity { get; set; }
}
