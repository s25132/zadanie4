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

        public async Task<int> AddProductToWarehouseAsync(Request addRequest)
        {
            return await _repo.AddProductToWarehouseAsync(addRequest);
        }

        public async Task<int> AddProductToWarehouseProcedureAsync(Request addRequest)
        {
            return await _repo.AddProductToWarehouseByProcedureAsync(addRequest);
        }
    }
}
