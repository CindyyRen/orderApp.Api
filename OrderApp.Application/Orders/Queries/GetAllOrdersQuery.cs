using MediatR;
using OrderApp.Application.DTOs.Order;
using OrderApp.Application.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Orders.Queries;

public sealed record GetAllOrdersQuery : IRequest<List<OrderDto>>;
