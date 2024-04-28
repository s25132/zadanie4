using Microsoft.AspNetCore.Mvc;
using Warehouse.Model;
using Warehouse.Services;

namespace Warehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseController : ControllerBase
    {
 
        private readonly ILogger<WarehouseController> _logger;
        private IProductService _productService;

        public WarehouseController(ILogger<WarehouseController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpPost(Name = "AddProductToWarehouse")]
        public IActionResult AddProductToWarehouse([FromBody] Request addRequest)
        {
            _logger.LogDebug("Request " + addRequest);
            return Ok(_productService.AddProductToWarehouse(addRequest));
        }
    }
}
