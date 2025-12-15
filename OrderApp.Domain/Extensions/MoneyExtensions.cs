using OrderApp.Domain.ValueObjects;
using System;

namespace OrderApp.Domain.Extensions;

internal static class MoneyExtensions
{
    public static Money ApplyDiscount(this Money price, decimal percentage)
    {
        if (percentage < 0 || percentage > 1)
            throw new ArgumentOutOfRangeException(nameof(percentage));

        var discountedCents = (long)(price.Cents * (1 - percentage));
        return Money.FromCents(discountedCents, price.Currency);
    }

    public static Money AddTax(this Money amount, decimal taxRate)
    {
        var taxCents = (long)(amount.Cents * taxRate);
        return Money.FromCents(amount.Cents + taxCents, amount.Currency);
    }
}
