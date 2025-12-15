using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApp.Domain.Entities;
using System.Reflection.Emit;

namespace OrderApp.Infrastructure.Data.Configurations;

// ✅ 改为 public
public sealed class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(t => t.Description)
               .HasMaxLength(1000);

        // ⚠️ 如果 Price 是 Money 类型，需要特殊处理（见下文）
        builder.Property(t => t.Price)
               .HasColumnType("decimal(18,2)");
    }
}
