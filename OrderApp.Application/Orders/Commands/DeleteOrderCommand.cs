using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Orders.Commands;

public sealed record DeleteOrderCommand(Guid OrderId)
    : IRequest<Unit>;
