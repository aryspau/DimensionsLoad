using DimensionsLoadConsole.Entities.DWHNorthwindOrders;
using Microsoft.EntityFrameworkCore;

namespace DimensionsLoadConsole.Contexts.DWHNorthwindOrders;

public partial class DWHNorthwindOrdersContext : DbContext
{
    public DWHNorthwindOrdersContext(DbContextOptions<DWHNorthwindOrdersContext> options)
        : base(options)
    {
    }

    public DbSet<DimCategory> DimCategories { get; set; }
    public DbSet<DimCustomer> DimCustomers { get; set; }
    public DbSet<DimEmployee> DimEmployees { get; set; }
    public DbSet<DimProduct> DimProducts { get; set; }
    public DbSet<DimShipper> DimShippers { get; set; }
    public DbSet<FactClientesAtendido> FactClientesAtendidos { get; set; }
    public DbSet<FactOrder> FactOrders { get; set; }
}
