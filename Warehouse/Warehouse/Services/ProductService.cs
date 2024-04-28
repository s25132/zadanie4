using Warehouse.Model;
using Warehouse.Repositories;

namespace Warehouse.Services
{
    public class ProductService : IProductService
    {

        private IProductRepo _repo;

        public ProductService(IProductRepo repo) 
        {  
            _repo = repo; 
        }

        public int AddProductToWarehouse(Request addRequest)
        {
            return _repo.AddProductToWarehouse(addRequest);
        }
    }
}
