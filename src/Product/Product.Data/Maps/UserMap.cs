using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Data.Maps.Base;
using Product.Domain.Entities;

namespace Product.Data.Maps
{
    public class UserMap : BaseMap<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            base.Configure(builder);

            builder.Property(p => p.Name)
                .HasColumnType("varchar(250)")
                .HasColumnName("Name")
                .IsRequired();

            builder.Property(p => p.Email)
                .HasColumnType("varchar(250)")
                .HasColumnName("Email")
                .IsRequired();

            builder.Property(p => p.Password)
                .HasColumnType("varchar(1000)")
                .HasColumnName("Password")
                .IsRequired();

            builder.HasIndex(i => i.Email).IsUnique();
        }
    }
}
