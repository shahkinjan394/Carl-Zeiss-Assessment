using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Data.Entities;

namespace ProductManagement.Service.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task<AddProductResponse> AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(string id);
    }
}
