using SmartChargingPoC.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace SmartChargingPoC.DataAccess.Contexts
{
    public class SmartChargingContext : DbContext
    {
        public DbSet<ChargeStation> ChargeStations { get; set; }
        public DbSet<Connector> Connectors { get; set; }
        public DbSet<Group> Groups { get; set; }

        public SmartChargingContext(DbContextOptions<SmartChargingContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region One-to-Many Relations

            //One-to-Many Relationship between ChargeStation and Group
            modelBuilder.Entity<ChargeStation>()
                        .HasOne(x => x.Group)
                        .WithMany(x => x.ChargeStations)
                        .HasForeignKey(x => x.GroupId)
                        .OnDelete(DeleteBehavior.Cascade);

            //One-to-Many Relationship between Connector and ChargeStation
            modelBuilder.Entity<Connector>()
                        .HasOne(x => x.ChargeStation)
                        .WithMany(x => x.Connectors)
                        .HasForeignKey(x => x.ChargeStationId)
                        .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Constraints 

            modelBuilder.Entity<Connector>()
                        .HasIndex(c => new { c.ChargeStationId, ChargeStationNumber = c.ChargeStationUniqueNumber })
                        .IsUnique();

            #endregion
        }
    }
}