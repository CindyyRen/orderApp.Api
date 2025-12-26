using MediatR;
using OrderApp.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Orders.Commands;

public sealed record UpdateOrderCommand(
    Guid OrderId,
    InputOrderDto Input
) : IRequest<Unit>;