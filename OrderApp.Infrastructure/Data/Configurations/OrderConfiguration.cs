using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Data.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.CustomerId).IsRequired();

        builder.Property(o => o.Status)
               .HasConversion<int>();

        builder.Property(o => o.CreatedAt).IsRequired();

        builder.HasMany(o => o.Items)
       .WithOne(oi => oi.Order)
       .HasForeignKey(oi => oi.OrderId)
       .IsRequired()
       .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(o => o.Items)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(o => o.TotalPrice);
    }
}
