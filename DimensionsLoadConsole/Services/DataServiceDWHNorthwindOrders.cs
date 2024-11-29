using DimensionsLoadConsole.Contexts.Northwind;
using DimensionsLoadConsole.Contexts.DWHNorthwindOrders;
using DimensionsLoadConsole.Core;
using DimensionsLoadConsole.Interface;
using DimensionsLoadConsole.Entities.DWHNorthwindOrders;
using DimensionsLoadConsole.Entities.Northwind;
using Microsoft.EntityFrameworkCore;

namespace DimensionsLoadConsole.Services
{
    public class DataServiceDWHNorthwindOrders : IDataServiceDWHNorthwindOrders
    {
        private readonly NorthwindContext _northwindContext;
        private readonly DWHNorthwindOrdersContext _dwhNorthwindOrdersContext;

        public DataServiceDWHNorthwindOrders(NorthwindContext northwindContext,
                                   DWHNorthwindOrdersContext dwhNorthwindOrdersContext)
        {
            _northwindContext = northwindContext;
            _dwhNorthwindOrdersContext = dwhNorthwindOrdersContext;
        }

        public async Task<OperationResult> LoadDHW()
        {
            OperationResult result = new OperationResult();
            try
            {
                //await LoadCategory();
                //await LoadCustomer();
                //await LoadEmployee();
                //await LoadProduct();
                await LoadShipper();
                //await loadClienteAtentidos();
                //await LoadFactOrders();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando el DWH Ventas. {ex.Message}";
            }

            return result;
        }

        private async Task<OperationResult> LoadCategory()
        {
            OperationResult operation = new OperationResult();
            try
            {
                // Eliminar datos existentes
                await _dwhNorthwindOrdersContext.DimCategories.ExecuteDeleteAsync();

                // Cargar nuevas categorías
                var categories = await _northwindContext.Categories.Select(cat => new DimCategory()
                {
                    CategoryName = cat.CategoryName,
                    CategoryId = cat.CategoryId
                }).AsNoTracking().ToListAsync();

                await _dwhNorthwindOrdersContext.DimCategories.AddRangeAsync(categories);
                await _dwhNorthwindOrdersContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operation.Success = false;
                operation.Message = $"Error cargando dimensión de categoría. {ex.Message}";
            }
            return operation;
        }

        private async Task<OperationResult> LoadCustomer()
        {
            OperationResult operation = new OperationResult();
            try
            {
                // Eliminar datos existentes
                await _dwhNorthwindOrdersContext.DimCustomers.ExecuteDeleteAsync();

                // Obtener y cargar clientes
                var customers = await _northwindContext.Customers.Select(cust => new DimCustomer()
                {
                    CustomerName = cust.ContactName,
                    CustomerId = cust.CustomerId
                }).AsNoTracking().ToListAsync();

                await _dwhNorthwindOrdersContext.DimCustomers.AddRangeAsync(customers);
                await _dwhNorthwindOrdersContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operation.Success = false;
                operation.Message = $"Error cargando dimensión de cliente. {ex.Message}";
            }
            return operation;
        }

        private async Task<OperationResult> LoadEmployee()
        {
            OperationResult operation = new OperationResult();
            try
            {
                // Eliminar datos existentes
                await _dwhNorthwindOrdersContext.DimEmployees.ExecuteDeleteAsync();

                // Obtener y cargar empleados
                var employees = await _northwindContext.Employees.Select(emp => new DimEmployee()
                {
                    EmployeeId = emp.EmployeeId,
                    EmployeeName = string.Concat(emp.FirstName, " ", emp.LastName)
                }).AsNoTracking().ToListAsync();

                await _dwhNorthwindOrdersContext.DimEmployees.AddRangeAsync(employees);
                await _dwhNorthwindOrdersContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operation.Success = false;
                operation.Message = $"Error cargando dimensión de empleado. {ex.Message}";
            }
            return operation;
        }

        private async Task<OperationResult> LoadProduct()
        {
            OperationResult operation = new OperationResult();
            try
            {
                // Eliminar datos existentes
                await _dwhNorthwindOrdersContext.DimProducts.ExecuteDeleteAsync();

                // Obtener y cargar productos
                var products = await _northwindContext.Products.Select(prod => new DimProduct()
                {
                    ProductName = prod.ProductName,
                    ProductId = prod.ProductId
                }).AsNoTracking().ToListAsync();

                await _dwhNorthwindOrdersContext.DimProducts.AddRangeAsync(products);
                await _dwhNorthwindOrdersContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operation.Success = false;
                operation.Message = $"Error cargando dimensión de producto. {ex.Message}";
            }
            return operation;
        }

        private async Task<OperationResult> LoadShipper()
        {
            OperationResult operation = new OperationResult();
            try
            {
                // Eliminar datos existentes
                await _dwhNorthwindOrdersContext.DimShippers.ExecuteDeleteAsync();

                // Obtener y cargar transportistas
                var shippers = await _northwindContext.Shippers.Select(ship => new DimShipper()
                {
                    ShipperId = ship.ShipperId,
                    ShipperName = ship.CompanyName
                }).AsNoTracking().ToListAsync();

                await _dwhNorthwindOrdersContext.DimShippers.AddRangeAsync(shippers);
                await _dwhNorthwindOrdersContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operation.Success = false;
                operation.Message = $"Error cargando dimensión de transportista. {ex.Message}";
            }
            return operation;
        }
        private async Task<OperationResult> LoadFactOrders()
        {
            OperationResult result = new OperationResult();

            try
            {

                var ventas = await _northwindContext.Vwventas.AsNoTracking().ToListAsync();



                int[] ordersId = await _dwhNorthwindOrdersContext.FactOrders.Select(cd => cd.OrderNumber).ToArrayAsync();

                if (ordersId.Any())
                {
                    await _dwhNorthwindOrdersContext.FactOrders.Where(cd => ordersId.Contains(cd.OrderNumber))
                                                  .AsNoTracking()
                                                  .ExecuteDeleteAsync();
                }

                foreach (var venta in ventas)
                {
                    var customer = await _dwhNorthwindOrdersContext.DimCustomers.SingleOrDefaultAsync(cust => cust.CustomerId == venta.CustomerId);
                    var employee = await _dwhNorthwindOrdersContext.DimEmployees.SingleOrDefaultAsync(emp => emp.EmployeeId == venta.EmployeeId);
                    var shippers = await _dwhNorthwindOrdersContext.DimShippers.SingleOrDefaultAsync(ship => ship.ShipperId == venta.ShipperId);
                    var product = await _dwhNorthwindOrdersContext.DimProducts.SingleOrDefaultAsync(pro => pro.ProductId == venta.ProductId);


                    FactOrder factOrder = new FactOrder()
                    {
                        CantidadVentas = venta.Cantidad.HasValue ? Math.Floor((decimal)venta.Cantidad.Value) : 0, // Conversión explícita a decimal
                        Country = venta.Country ?? string.Empty, // Evitar nulos para Country
                        CustomerKey = customer?.CustomerKey ?? 0, // Validar que el cliente exista
                        EmployeeKey = employee?.EmployeeKey ?? 0, // Validar que el empleado exista
                        ProductKey = product?.ProductKey ?? 0,   // Validar que el producto exista
                        ShipperKey = shippers?.ShipperKey ?? 0,   // Validar que el transportista exista
                        TotalVentas = venta.TotalVentas.HasValue ? (decimal)venta.TotalVentas.Value : 0m // Manejar nulos
                    };


                    await _dwhNorthwindOrdersContext.FactOrders.AddAsync(factOrder);

                    await _dwhNorthwindOrdersContext.SaveChangesAsync();
                }



                result.Success = true;
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando el fact de ventas {ex.Message} ";
            }

            return result;
        }
        private async Task<OperationResult> loadClienteAtentidos()
        {
            OperationResult operaction = new OperationResult() { Success = true };

            try
            {
                var customerServeds = await _northwindContext.VwclientEmployees.AsNoTracking().ToListAsync();

                // Obtener las claves de tipo string para los clientes atendidos
                var customers = _dwhNorthwindOrdersContext.FactClientesAtendidos
                                                 .Select(cust => cust.ClienteAtendidoKey)
                                                 .ToArray();

                // Limpiar la tabla de facts
                if (customers.Any())
                {
                    await _dwhNorthwindOrdersContext.FactClientesAtendidos
                                           .Where(fact => customers.Contains(fact.ClienteAtendidoKey))
                                           .AsNoTracking()
                                           .ExecuteDeleteAsync();
                }

                // Cargar el fact de clientes atendidos
                foreach (var customer in customerServeds)
                {
                    // Buscar el empleado asociado con el cliente atendido
                    var employee = await _dwhNorthwindOrdersContext.DimEmployees
                                                          .SingleOrDefaultAsync(emp => emp.EmployeeId == customer.EmployeeKey);

                    // Crear un nuevo registro en FactClientesAtendidos
                    FactClientesAtendido factClienteAtendido = new FactClientesAtendido()
                    {
                        // Asegurarte de que ClienteAtendidoKey se mantenga como string
                        ClienteAtendidoKey = customer.ClienteAtendidoKey, // Mantener como string
                        EmployeeKey = employee.EmployeeKey, // Asegurarse de manejar nulos
                        TotalClientes = customer.TotalClientes
                    };

                    await _dwhNorthwindOrdersContext.FactClientesAtendidos.AddAsync(factClienteAtendido);
                    await _dwhNorthwindOrdersContext.SaveChangesAsync();
                }

                operaction.Success = true;
            }
            catch (Exception ex)
            {
                operaction.Success = false;
                operaction.Message = $"Error cargando el fact de clientes atendidos {ex.Message} ";
            }

            return operaction;
        }
    }
}