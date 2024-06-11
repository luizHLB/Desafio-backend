using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Data.Maps.Base;
using Product.Domain.Entities;

namespace Product.Data.Maps
{
    public class VehicleMap : BaseMap<Vehicle>, IEntityTypeConfiguration<Vehicle>
    {
        public override void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicle");

            base.Configure(builder);

            builder.Property(p => p.Identifier)
                .HasColumnType("varchar(250)")
                .HasColumnName("Identifier")
                .IsRequired();

            builder.Property(p => p.Year)
                .HasColumnType("int")
                .HasColumnName("Year")
                .IsRequired();
        }
    }
}
