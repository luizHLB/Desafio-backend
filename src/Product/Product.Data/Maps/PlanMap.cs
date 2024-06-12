using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Data.Maps.Base;
using Product.Domain.Entities;

namespace Product.Data.Maps
{
    public class PlanMap : BaseMap<Plan>
    {
        public override void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.ToTable("Plan");

            base.Configure(builder);

            builder.Property(p => p.Name)
                .HasColumnType("varchar(250)")
                .HasColumnName("Name")
                .IsRequired();

            builder.Property(p => p.Period)
                .HasColumnType("int")
                .HasColumnName("Period")
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("numeric(10,2)")
                .HasColumnName("Price")
                .IsRequired();

            builder.Property(p => p.Fine)
                .HasColumnType("numeric(10,2)")
                .HasColumnName("Fine")
                .IsRequired(false);

            builder.Property(p => p.Extra)
                .HasColumnType("numeric(10,2)")
                .HasColumnName("Extra")
                .IsRequired(false);

        }
    }
}
