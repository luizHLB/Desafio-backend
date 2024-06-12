using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Data.Maps.Base;
using Product.Domain.Entities;

namespace Product.Data.Maps
{
    public class RentalMap : BaseMap<Rental>
    {
        public override void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.ToTable("Rental");

            base.Configure(builder);

            builder.Property(p => p.DriverId)
                .HasColumnType("bigint")
                .HasColumnName("DriverId")
                .IsRequired();

            builder.Property(p => p.VehicleId)
                .HasColumnType("bigint")
                .HasColumnName("VehicleId")
                .IsRequired();

            builder.Property(p => p.PlanId)
               .HasColumnType("bigint")
               .HasColumnName("PlanId")
               .IsRequired();

            builder.Property(p => p.WithdrawDate)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("WithdrawDate")
                .IsRequired();

            builder.Property(p => p.ReturnDate)
               .HasColumnType("timestamp with time zone")
               .HasColumnName("ReturnDate")
               .IsRequired(false);

            builder.Property(p => p.EstimatedReturnDate)
               .HasColumnType("timestamp with time zone")
               .HasColumnName("EstimatedReturnDate")
               .IsRequired();

            builder.HasOne(o => o.Driver)
                .WithMany(m => m.Rentals)
                .HasForeignKey(fk => fk.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.Vehicle)
                .WithMany(m => m.Rentals)
                .HasForeignKey(fk => fk.VehicleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.Plan)
                .WithMany(m => m.Rentals)
                .HasForeignKey(fk => fk.PlanId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
