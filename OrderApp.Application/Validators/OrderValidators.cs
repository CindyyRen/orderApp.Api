using OrderApp.Application.DTOs.Order;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Validators;

public sealed class InputOrderDtoValidator : AbstractValidator<InputOrderDto>
{
    public InputOrderDtoValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("订单至少需要包含一个商品");

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemInputValidator());
    }
}

public sealed class OrderItemInputValidator : AbstractValidator<InputOrderItemDto>
{
    public OrderItemInputValidator()
    {
        RuleFor(x => x.MenuItemId)
            .NotEmpty()
            .WithMessage("商品ID不能为空");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("数量必须大于0")
            .LessThanOrEqualTo(99)
            .WithMessage("单次订购数量不能超过99");
    }
}
