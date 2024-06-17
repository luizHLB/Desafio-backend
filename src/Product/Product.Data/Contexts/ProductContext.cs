﻿using Microsoft.EntityFrameworkCore;
using Product.Data.Maps;
using Product.Domain.Entities;

namespace Product.Data.Contexts
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) 
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Vehicle>(new VehicleMap().Configure);
            modelBuilder.Entity<Driver>(new DriverMap().Configure);
            modelBuilder.Entity<Plan>(new PlanMap().Configure);
            modelBuilder.Entity<Rental>(new RentalMap().Configure);
            modelBuilder.Entity<Notification>(new NotificationMap().Configure);
            modelBuilder.Entity<User>(new UserMap().Configure);
        }
    }
}
