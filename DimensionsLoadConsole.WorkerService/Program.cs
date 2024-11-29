using DimensionsLoadConsole.WorkerService;
using Microsoft.EntityFrameworkCore;
using DimensionsLoadConsole.Services;
using DimensionsLoadConsole.Interface;
using DimensionsLoadConsole.Contexts.DWHNorthwindOrders;
using DimensionsLoadConsole.Contexts.Northwind;

internal class Program
{
    private static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) => {

            services.AddDbContextPool<NorthwindContext>(options =>
                                                      options.UseSqlServer(hostContext.Configuration.GetConnectionString("Northwind")));

            services.AddDbContextPool<DWHNorthwindOrdersContext>(options =>
                                                      options.UseSqlServer(hostContext.Configuration.GetConnectionString("DWHNorthwindOrders")));


            services.AddScoped<IDataServiceDWHNorthwindOrders, DataServiceDWHNorthwindOrders>();


            services.AddHostedService<Worker>();
        });
}
