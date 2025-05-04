using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Data.Entities;
using Azure;

namespace ProductManagement.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Product>> GetAllAsync() => await _context.Products.ToListAsync();
        public async Task<Product> GetByIdAsync(string id) => await _context.Products.FindAsync(id);
        public async Task<AddProductResponse> AddAsync(Product product)
        {
            var response = new AddProductResponse();

            try
            {
                product.ProductId = Convert.ToString(await GenerateUniqueId());
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                response.Message = "Product added successfully.";
                response.ProductId = product.ProductId;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = "An error occurred while adding the product.";
                response.IsSuccess = false;
            }
            return response;
        }

        private async Task<string> GenerateUniqueId()
        {
            string id;
            do
            {
                id = new Random().Next(100000, 999999).ToString();
            }
            while (await _context.Products.AnyAsync(p => p.ProductId == id));
            return id;
        }
        public async Task UpdateAsync(Product product)
        {
            var response = new AddProductResponse();

            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                response.Message = "Product Updated successfully.";               
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = "An error occurred while adding the product.";
                response.IsSuccess = false;
            }

        }
        public async Task DeleteAsync(string id)
        {
            var response = new AddProductResponse();

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();

                    response.Message = "Product Deleted successfully.";
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = "An error occurred while adding the product.";
                response.IsSuccess = false;
            }

        }
        public async Task<bool> ExistsAsync(string id) => await _context.Products.AnyAsync(p => p.ProductId == id);

    }
}
