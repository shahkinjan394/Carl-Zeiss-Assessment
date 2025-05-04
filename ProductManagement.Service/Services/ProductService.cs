using ProductManagement.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Data.Entities;
using ProductManagement.Data.Repositories;

namespace ProductManagement.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() => await _repository.GetAllAsync();
        public async Task<Product> GetProductByIdAsync(string id) => await _repository.GetByIdAsync(id);
        public async Task<AddProductResponse> AddProductAsync(Product product) => await _repository.AddAsync(product);
        public async Task UpdateProductAsync(Product product) => await _repository.UpdateAsync(product);
        public async Task DeleteProductAsync(string id) => await _repository.DeleteAsync(id);

    }
}
