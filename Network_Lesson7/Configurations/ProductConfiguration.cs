using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Network_Lesson7_Shared.Models;
namespace Network_Lesson7_Shared.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .HasColumnType("NVARCHAR(100)");

        builder.Property(x => x.Category)
            .HasMaxLength(100)
            .HasColumnType("NVARCHAR(100)");

        builder.HasQueryFilter(x => x.Price > 0);
    }

}
