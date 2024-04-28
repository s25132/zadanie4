using Warehouse.Model;

namespace Warehouse.Services
{
    public interface IProductService
    {
        int AddProductToWarehouse(Request addRequest);
    }
}
