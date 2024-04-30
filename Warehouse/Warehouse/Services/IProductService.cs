using Warehouse.Model;

namespace Warehouse.Services
{
    public interface IProductService
    {
       Task<int> AddProductToWarehouse(Request addRequest);
    }
}
