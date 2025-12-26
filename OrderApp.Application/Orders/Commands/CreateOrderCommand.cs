using MediatR;
using OrderApp.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Orders.Commands;

public sealed record CreateOrderCommand(InputOrderDto Input) : IRequest<Guid>;