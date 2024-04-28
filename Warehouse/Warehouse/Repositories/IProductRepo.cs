using Warehouse.Model;

namespace Warehouse.Repositories
{
    public interface IProductRepo
    {
        int AddProductToWarehouse(Request addRequest);
    }
}
