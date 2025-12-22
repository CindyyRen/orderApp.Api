using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Data.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.MenuItemId).IsRequired();

        builder.Property(oi => oi.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(oi => oi.Quantity).IsRequired();

        // ⚠️ Money 类型处理
        builder.OwnsOne(oi => oi.Price, p =>
        {
            p.Property(x => x.Cents).HasColumnName("PriceCents").IsRequired();
            p.Property(x => x.Currency).HasColumnName("PriceCurrency").HasMaxLength(3).IsRequired();
        });

        // 外键到 Order
        builder.HasOne(oi => oi.Order)
               .WithMany(o => o.Items)
               .HasForeignKey(oi => oi.OrderId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

    }
}