using DimensionsLoadConsole.Core;

namespace DimensionsLoadConsole.Interface
{
    public interface IDataServiceDWHNorthwindOrders
    {
        Task<OperationResult> LoadDHW();
    }
}
