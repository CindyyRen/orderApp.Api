using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.DTOs.Order;

public sealed class InputOrderDto
{
        public Collection<InputOrderItemDto> Items { get; init; } = new();
}