using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApp.Domain.Entities;
using OrderApp.Domain.ValueObjects;

namespace OrderApp.Infrastructure.Data.Configurations;

public sealed class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(t => t.Description)
               .HasMaxLength(1000);

        // ⚠️ 配置 Owned Entity Money
        /*            builder.OwnsOne(m => m.Price, priceBuilder =>
                    {
                        priceBuilder.Property(p => p.Cents).HasColumnName("PriceCents");
                        priceBuilder.Property(p => p.Currency).HasColumnName("PriceCurrency");
                    });*/
        // ⚡ 将 Money 类型映射为 decimal 存数据库
        // ✅ 使用 HasConversion
        builder.Property(m => m.Price)
               .HasConversion(
                   m => m.ToDecimal(),                    // Money → decimal
                   v => Money.FromDollars(v, "AUD")      // decimal → Money
               )
               .HasColumnType("decimal(18,2)")
               .IsRequired();

    }
}