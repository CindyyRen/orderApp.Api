using OrderApp.Domain.Common;
using System;
using System.Collections.Generic;

namespace OrderApp.Domain.ValueObjects;

public sealed class Money : ValueObject, IComparable<Money>, IEquatable<Money>
{
    // 1. 属性 / 常量
    public long Cents { get; }
    public string Currency { get; }
    public static readonly Money Zero = new(0, "AUD");

    // 2. 构造函数
    public Money(long cents, string currency)
    {
        if (cents < 0)
            throw new ArgumentOutOfRangeException(nameof(cents), "Money cannot be negative");

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));

        Cents = cents;
        Currency = currency.ToUpperInvariant();  // 统一大写
    }

    // 3. 工厂方法
    public static Money FromCents(long cents, string currency = "AUD")
        => new Money(cents, currency);

    public static Money FromDollars(decimal dollars, string currency = "AUD")
    {
        var cents = (long)Math.Round(dollars * 100, MidpointRounding.AwayFromZero);
        return new Money(cents, currency);
    }

    // 4. 领域行为（Domain Methods）
    public Money Add(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);
        EnsureSameCurrency(this, other);
        return new Money(Cents + other.Cents, Currency);
    }

    public Money Subtract(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);
        EnsureSameCurrency(this, other);
        if (Cents < other.Cents)
            throw new InvalidOperationException("Resulting money cannot be negative");
        return new Money(Cents - other.Cents, Currency);
    }

    public Money Multiply(int factor)
    {
        if (factor < 0)
            throw new ArgumentOutOfRangeException(nameof(factor), "Factor cannot be negative");
        return new Money(Cents * factor, Currency);
    }

    // 5. 内部辅助方法
    private static void EnsureSameCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException(
                $"Cannot operate on different currencies: {left.Currency} vs {right.Currency}");
    }

    // 6. 运算符重载

    public static Money operator +(Money left, Money right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot add different currencies");

        return new Money(left.Cents + right.Cents, left.Currency);
    }
    public static Money operator -(Money left, Money right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.Subtract(right);
    }

    public static Money operator *(Money money, int quantity)
    {
        ArgumentNullException.ThrowIfNull(money);
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantity));

        return new Money(money.Cents * quantity, money.Currency);
    }


    public static Money operator *(int factor, Money money)
    {
        ArgumentNullException.ThrowIfNull(money);
        return money.Multiply(factor);
    }

    public static bool operator >(Money? left, Money? right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.CompareTo(right) > 0;
    }

    public static bool operator <(Money? left, Money? right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.CompareTo(right) < 0;
    }

    public static bool operator >=(Money left, Money right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.CompareTo(right) >= 0;
    }

    public static bool operator <=(Money left, Money right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.CompareTo(right) <= 0;
    }

    public static bool operator ==(Money? left, Money? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Money? left, Money? right)
        => !(left == right);

    // 7. 接口实现
    public bool Equals(Money? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Cents == other.Cents && Currency == other.Currency;
    }

    public int CompareTo(Money? other)
    {
        if (other is null) return 1;
        EnsureSameCurrency(this, other);
        return Cents.CompareTo(other.Cents);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Cents;
        yield return Currency;
    }

    // 8. 重写 Object 方法
    public override bool Equals(object? obj)
        => obj is Money other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Cents, Currency);

    public override string ToString()
    {
        var dollars = Cents / 100;
        var cents = Cents % 100;
        return $"{Currency} {dollars}.{cents:D2}";
    }

    // 9. 可选辅助方法
    public decimal ToDecimal() => Cents / 100m;
}
