using Warehouse.Model;

namespace Warehouse.Repositories
{
    public interface IProductRepo
    {
        Task<int> AddProductToWarehouseAsync(Request addRequest);
        Task<int> AddProductToWarehouseByProcedureAsync(Request addRequest);
    }
}
