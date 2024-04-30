using Warehouse.Model;

namespace Warehouse.Services
{
    public interface IProductService
    {
       Task<int> AddProductToWarehouseAsync(Request addRequest);
       Task<int> AddProductToWarehouseProcedureAsync(Request addRequest);
    }
}
