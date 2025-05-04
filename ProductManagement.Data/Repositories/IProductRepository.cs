using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Data.Entities;

namespace ProductManagement.Data.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(string id);
        Task<AddProductResponse> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}
