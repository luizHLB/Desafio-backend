using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Data.Maps.Base;
using Product.Domain.Entities;

namespace Product.Data.Maps
{
    public class NotificationMap : BaseMap<Notification>
    {
        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");

            base.Configure(builder);

            builder.Property(p => p.Message)
                .HasColumnType("varchar(500)")
                .HasColumnName("Message")
                .IsRequired();

            builder.Property(p => p.Read)
                .HasColumnType("boolean")
                .HasColumnName("Read")
                .IsRequired();

        }
    }
}
