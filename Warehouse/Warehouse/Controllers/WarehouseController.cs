using Microsoft.AspNetCore.Mvc;
using Warehouse.Model;
using Warehouse.Services;

namespace Warehouse.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WarehouseController : ControllerBase
    {
 
        private IProductService _productService;

        public WarehouseController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost(Name = "AddProductToWarehouseAsync")]
        public async Task<IActionResult> AddProductToWarehouseAsync([FromBody] Request addRequest)
        {
            try
            {
                int result = await _productService.AddProductToWarehouseAsync(addRequest);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return getError(ex);
            }
        }

        [HttpPost(Name = "AddProductToWarehouseProcedureAsync")]
        public async Task<IActionResult> AddProductToWarehouseProcedureAsync([FromBody] Request addRequest)
        {
            try
            {
                int result = await _productService.AddProductToWarehouseProcedureAsync(addRequest);
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