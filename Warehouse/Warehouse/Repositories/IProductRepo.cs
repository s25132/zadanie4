using Warehouse.Model;

namespace Warehouse.Repositories
{
    public interface IProductRepo
    {
        Task<int> AddProductToWarehouse(Request addRequest);
    }
}
