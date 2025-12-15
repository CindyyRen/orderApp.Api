using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.DTOs.Menu;

// DTO（数据传输对象）用于 API 接口请求
public record MenuItemDto(
    string Name,
    decimal Price,
    MenuCategory Category,
    string? Description
);
