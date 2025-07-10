using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets – All 10 Models
        public DbSet<User> Users { get; set; }
        public DbSet<BusOperator> BusOperators { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<BusAmenity> BusAmenities { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  Unique constraint for Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            //  One-to-One: User ↔ Operator
            modelBuilder.Entity<User>()
                .HasOne(u => u.BusOperatorProfile)
                .WithOne(o => o.User)
                .HasForeignKey<BusOperator>(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //  One-to-Many: Route ↔ Buses
            modelBuilder.Entity<Route>()
                .HasMany(r => r.Buses)
                .WithOne(b => b.Route)
                .HasForeignKey(b => b.RouteId);

            //  One-to-Many: BusOperator ↔ Buses
            modelBuilder.Entity<BusOperator>()
                .HasMany(o => o.Buses)
                .WithOne(b => b.BusOperator)
                .HasForeignKey(b => b.BusOperatorId);

            //  One-to-Many: Bus ↔ Seats
            modelBuilder.Entity<Bus>()
                .HasMany(b => b.Seats)
                .WithOne(s => s.Bus)
                .HasForeignKey(s => s.BusId);

            //  One-to-Many: User ↔ Bookings
            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);

            //  One-to-Many: Bus ↔ Bookings
            modelBuilder.Entity<Bus>()
                .HasMany(b => b.Bookings)
                .WithOne(bk => bk.Bus)
                .HasForeignKey(bk => bk.BusId);

            //  One-to-One: Booking ↔ Payment
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingId);

            //  One-to-One: Booking ↔ Cancellation
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Cancellation)
                .WithOne(c => c.Booking)
                .HasForeignKey<Cancellation>(c => c.BookingId);

            //  Many-to-Many: Bus ↔ Amenity via BusAmenity
            modelBuilder.Entity<BusAmenity>()
                .HasKey(ba => new { ba.BusId, ba.AmenityId });

            modelBuilder.Entity<BusAmenity>()
                .HasOne(ba => ba.Bus)
                .WithMany(b => b.BusAmenities)
                .HasForeignKey(ba => ba.BusId);

            modelBuilder.Entity<BusAmenity>()
                .HasOne(ba => ba.Amenity)
                .WithMany(a => a.BusAmenities)
                .HasForeignKey(ba => ba.AmenityId);

            // Add default data for Amenities
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { Id = 1, Name = "Blanket" },
                new Amenity { Id = 2, Name = "Charging Port" },
                new Amenity { Id = 3, Name = "TV" },
                new Amenity { Id = 4, Name = "Water Bottle" }
            );
        }
    }
}
