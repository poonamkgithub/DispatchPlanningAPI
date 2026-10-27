using DispatchPlanningApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DispatchPlanningApi.Data;

public class DispatchDbContext : DbContext
{
    public DispatchDbContext(DbContextOptions<DispatchDbContext> options) : base(options) { }
    public DbSet<DispatchPlan> DispatchPlans => Set<DispatchPlan>();
    public DbSet<Shipment> Shipments => Set<Shipment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DispatchPlan>().HasIndex(x => x.DispatchNumber).IsUnique();
        modelBuilder.Entity<Shipment>().HasIndex(x => x.TrackingNumber).IsUnique();
        base.OnModelCreating(modelBuilder);
    }
}
