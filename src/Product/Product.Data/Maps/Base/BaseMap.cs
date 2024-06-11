using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities.Base;

namespace Product.Data.Maps.Base
{
    public class BaseMap<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(pk => pk.Id);
            builder.Property(p => p.Id)
                .HasColumnType("bigint")
                .HasColumnName("Id")
                .HasIdentityOptions(1, 1, 1, long.MaxValue, false, 1);

            builder.Property(p => p.CreatedBy)
                .HasColumnType("bigint")
                .HasColumnName("CreatedBy")
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("CreatedAt")
                .HasDefaultValueSql("NOW()")
                .IsRequired();

            builder.Property(p => p.UpdatedBy)
                .HasColumnType("bigint")
                .HasColumnName("UpdatedBy")
                .HasDefaultValue(null);

            builder.Property(p => p.UpdatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("UpdatedAt")
                .HasDefaultValue(null);
        }
    }
}
