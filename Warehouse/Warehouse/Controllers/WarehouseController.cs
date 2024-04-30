using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Exceptions;
using Warehouse.Model;
using Warehouse.Services;

namespace Warehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarehouseController : ControllerBase
    {
 
        private IProductService _productService;

        public WarehouseController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost(Name = "AddProductToWarehouse")]
        public async Task<IActionResult> AddProductToWarehouse([FromBody] Request addRequest)
        {
            try
            {
                int result = await _productService.AddProductToWarehouse(addRequest);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return getError(ex);
            }
        }

        private ObjectResult getError(Exception ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
    }
}