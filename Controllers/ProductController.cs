using Auth.Model;
using Auth.Request;
using Auth.Services.IService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //to access anything you must provide a token
   
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProduct _product;

        public ProductController(IMapper mapper, IProduct product)
        {
            _mapper = mapper;
            _product = product;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductAsync() 
        {
            var products = await _product.GetProductsAsync();
            return Ok(products);
        }

        [HttpPost]
        //Allow only admin users to add products
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<string>> AddProductAsync(AddProduct newProduct)
        {
            var res = await _product.AddProductAsync(_mapper.Map<Product>(newProduct));
            return CreatedAtAction(nameof(AddProduct), res); 
        }
    }
}
