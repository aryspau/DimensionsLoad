using DimensionsLoadConsole.Entities.Northwind;
using Microsoft.EntityFrameworkCore;

namespace DimensionsLoadConsole.Contexts.Northwind;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Customer> Customers { get; set; }


    public DbSet<Employee> Employees { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Shipper> Shippers { get; set; }

    public DbSet<VwclientEmployee> VwclientEmployees { get; set; }

    public DbSet<Vwventa> Vwventas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VwclientEmployee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VwclientEmployee");


        });

        modelBuilder.Entity<Vwventa>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VWVentas");


        });
    }
}