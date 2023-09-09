using Auth.Data;
using Auth.Model;
using Auth.Services.IService;
using Microsoft.EntityFrameworkCore;
using Auth.Data;

namespace Auth.Services 
{
    public class ProductService : IProduct
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return "Product Added";
        }

        public  async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }

}
