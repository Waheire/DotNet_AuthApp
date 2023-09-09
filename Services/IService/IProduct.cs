using Auth.Model;

namespace Auth.Services.IService
{
    public interface IProduct
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<String> AddProductAsync(Product product);
    }
}
