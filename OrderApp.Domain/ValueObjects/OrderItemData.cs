using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Domain.ValueObjects;

public record OrderItemData(
    int MenuItemId,
    string Name,
    int PriceCents,
    int Quantity
);
