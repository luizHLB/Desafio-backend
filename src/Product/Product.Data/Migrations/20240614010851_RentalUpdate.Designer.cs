﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Product.Data.Contexts;

#nullable disable

namespace Product.Data.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20240614010851_RentalUpdate")]
    partial class RentalUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Product.Domain.Entities.Driver", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<long>("Id"), 1L, null, 1L, 9223372036854775807L, null, null);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("date")
                        .HasColumnName("BirthDate");

                    b.Property<string>("CNH")
                        .IsRequired()
                        .HasColumnType("varchar(9)")
                        .HasColumnName("CNH");

                    b.Property<short>("CNHCategory")
                        .HasColumnType("smallint")
                        .HasColumnName("CNHCategory");

                    b.Property<string>("CNHImage")
                        .IsRequired()
                        .HasColumnType("varchar(32)")
                        .HasColumnName("CNHImage");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnType("varchar(14)")
                        .HasColumnName("CNPJ");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt")
                        .HasDefaultValueSql("NOW()");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("Identifier");

                    b.Property<short>("ImageFormat")
                        .HasColumnType("smallint")
                        .HasColumnName("ImageFormat");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("Name");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAt");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("UpdatedBy");

                    b.HasKey("Id");

                    b.HasIndex("CNH")
                        .IsUnique();

                    b.HasIndex("CNPJ")
                        .IsUnique();

                    b.ToTable("Driver", (string)null);
                });

            modelBuilder.Entity("Product.Domain.Entities.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<long>("Id"), 1L, null, 1L, 9223372036854775807L, null, null);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt")
                        .HasDefaultValueSql("NOW()");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("Message");

                    b.Property<bool>("Read")
                        .HasColumnType("boolean")
                        .HasColumnName("Read");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAt");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("UpdatedBy");

                    b.HasKey("Id");

                    b.ToTable("Notification", (string)null);
                });

            modelBuilder.Entity("Product.Domain.Entities.Plan", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<long>("Id"), 1L, null, 1L, 9223372036854775807L, null, null);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt")
                        .HasDefaultValueSql("NOW()");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy");

                    b.Property<double?>("Extra")
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("Extra");

                    b.Property<double?>("Fine")
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("Fine");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("Name");

                    b.Property<int>("Period")
                        .HasColumnType("int")
                        .HasColumnName("Period");

                    b.Property<double>("Price")
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("Price");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAt");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("UpdatedBy");

                    b.HasKey("Id");

                    b.ToTable("Plan", (string)null);
                });

            modelBuilder.Entity("Product.Domain.Entities.Rental", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<long>("Id"), 1L, null, 1L, 9223372036854775807L, null, null);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt")
                        .HasDefaultValueSql("NOW()");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy");

                    b.Property<long>("DriverId")
                        .HasColumnType("bigint")
                        .HasColumnName("DriverId");

                    b.Property<DateTime>("EstimatedReturnDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("EstimatedReturnDate");

                    b.Property<long>("PlanId")
                        .HasColumnType("bigint")
                        .HasColumnName("PlanId");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ReturnDate");

                    b.Property<double?>("TotalExtras")
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("TotalExtras");

                    b.Property<double?>("TotalFines")
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("TotalFines");

                    b.Property<double?>("TotalRental")
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("TotalRental");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAt");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("UpdatedBy");

                    b.Property<long>("VehicleId")
                        .HasColumnType("bigint")
                        .HasColumnName("VehicleId");

                    b.Property<DateTime>("WithdrawDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("WithdrawDate");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("PlanId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Rental", (string)null);
                });

            modelBuilder.Entity("Product.Domain.Entities.Vehicle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<long>("Id"), 1L, null, 1L, 9223372036854775807L, null, null);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt")
                        .HasDefaultValueSql("NOW()");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("CreatedBy");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("Identifier");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasColumnType("varchar(8)")
                        .HasColumnName("LicensePlate");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Model");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedAt");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("UpdatedBy");

                    b.Property<int>("Year")
                        .HasColumnType("int")
                        .HasColumnName("Year");

                    b.HasKey("Id");

                    b.HasIndex("LicensePlate")
                        .IsUnique();

                    b.ToTable("Vehicle", (string)null);
                });

            modelBuilder.Entity("Product.Domain.Entities.Rental", b =>
                {
                    b.HasOne("Product.Domain.Entities.Driver", "Driver")
                        .WithMany("Rentals")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Product.Domain.Entities.Plan", "Plan")
                        .WithMany("Rentals")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Product.Domain.Entities.Vehicle", "Vehicle")
                        .WithMany("Rentals")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("Plan");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Product.Domain.Entities.Driver", b =>
                {
                    b.Navigation("Rentals");
                });

            modelBuilder.Entity("Product.Domain.Entities.Plan", b =>
                {
                    b.Navigation("Rentals");
                });

            modelBuilder.Entity("Product.Domain.Entities.Vehicle", b =>
                {
                    b.Navigation("Rentals");
                });
#pragma warning restore 612, 618
        }
    }
}
