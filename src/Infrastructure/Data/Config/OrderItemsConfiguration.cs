using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class OrderItemsConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.Property(b => b.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

        }
    }
}
