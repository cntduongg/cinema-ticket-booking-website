//using BookingService.Models;
//using BookingService.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace BookingService.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ProductController : Controller
//    {
//        private readonly IProductService _productService;

//        public ProductController(IProductService productService)
//        {
//            _productService = productService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllProducts()
//        {
//            var products = await _productService.GetAllAsync();
//            return Ok(products);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetProductById(int id)
//        {
//            var products = await _productService.GetByIdAsync(id);
//            return Ok(products);
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateProduct([FromBody] OrderProduct product)
//        {
//            try
//            {
//                var createdProduct = await _productService.CreateAsync(product);
//                return Ok(createdProduct);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//        }

//        [HttpPut]
//        public async Task<IActionResult> UpdateProduct([FromBody] OrderProduct product)
//        {
//            try
//            {
//                var updateProduct = await _productService.UpdateAsync(product);
//                return Ok(updateProduct);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//        }
//        [HttpDelete]
//        public async Task<IActionResult> DeleteProduct(int id)
//        {
//            try
//            {
//                var result = await _productService.DeleteAsync(id);
//                if (result)
//                {
//                    return Ok(new { message = "Product deleted successfully" });
//                }
//                return BadRequest(new { message = "Delete Product Error" });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new { message = ex.Message });
//            }
//        }
//    }
//}
