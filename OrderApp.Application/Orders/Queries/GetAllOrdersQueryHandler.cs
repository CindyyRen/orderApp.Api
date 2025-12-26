using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderApp.Application.DTOs.Order;
using OrderApp.Application.Mappings;
using OrderApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Orders.Queries;

public sealed class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllOrdersQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _dbContext.Orders
            .Include(o => o.Items)
            .ToListAsync(cancellationToken);

        return orders.Select(o => o.ToDto()).ToList();
    }
}
