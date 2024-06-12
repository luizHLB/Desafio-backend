using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Data.Maps.Base;
using Product.Domain.Entities;

namespace Product.Data.Maps
{
    public class DriverMap : BaseMap<Driver>
    {
        public override void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable("Driver");

            base.Configure(builder);

            builder.Property(p => p.Identifier)
                .HasColumnType("varchar(250)")
                .HasColumnName("Identifier")
                .IsRequired();

            builder.Property(p => p.Name)
                .HasColumnType("varchar(250)")
                .HasColumnName("Name")
                .IsRequired();

            builder.Property(p => p.CNPJ)
                .HasColumnType("varchar(14)")
                .HasColumnName("CNPJ")
                .IsRequired();

            builder.Property(p => p.BirthDate)
                .HasColumnType("date")
                .HasColumnName("BirthDate")
                .IsRequired();

            builder.Property(p => p.CNH)
                .HasColumnType("varchar(9)")
                .HasColumnName("CNH")
                .IsRequired();

            builder.Property(p => p.CNHCategory)
                .HasColumnType("smallint")
                .HasColumnName("CNHCategory")
                .IsRequired();

            builder.Property(p => p.CNHImage)
                .HasColumnType("varchar(32)")
                .HasColumnName("CNHImage")
                .IsRequired();

            builder.Property(p => p.ImageFormat)
                .HasColumnType("smallint")
                .HasColumnName("ImageFormat")
                .IsRequired();

            builder.HasIndex(i => i.CNPJ).IsUnique();
            builder.HasIndex(i => i.CNH).IsUnique();
        }
    }
}
