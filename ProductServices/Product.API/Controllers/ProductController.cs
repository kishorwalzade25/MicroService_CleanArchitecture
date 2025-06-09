using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Core.RepositriesService;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _product;

        public ProductController(IProductService product) 
        {
            _product = product;
        }

        [HttpGet]
        [Route("getProduct/{productId:int}")]
        public async Task<IActionResult> get_product(int productId) 
        {
            var products= await _product.GetProductByCondition(productId);
            if (products == null) {return NotFound();}  
            return Ok(products);
        }

        [HttpGet]
        [Route("getAllProduct")]
        public async Task<IActionResult> get_all_product() 
        {
            var products= await _product.GetProducts();
            if (products == null) {return NotFound();}
            return Ok(products);
        }
    }
}
