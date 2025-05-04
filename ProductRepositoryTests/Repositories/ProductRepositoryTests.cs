using Microsoft.EntityFrameworkCore;
using Moq;
using ProductManagement.Data.Entities;
using ProductManagement.Data.Repositories;
using ProductManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ProductRepositoryTests.Repositories
{
    public class ProductRepositoryTests
    {

        private readonly Mock<AppDbContext> _mockContext;
        private readonly ProductRepository _repository;
        private readonly Mock<DbSet<Product>> _mockSet;


        public ProductRepositoryTests()
        {
            _mockContext = new Mock<AppDbContext>();
            _mockSet = new Mock<DbSet<Product>>();

            _mockContext.Setup(c => c.Products).Returns(_mockSet.Object);
            _repository = new ProductRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductId = "123456", Name = "Product1" },
                new Product { ProductId = "654321", Name = "Product2" }
            }.AsQueryable();


            _mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { ProductId = "872522", Name = "Product1" };
            _mockSet.Setup(m => m.FindAsync("872522")).ReturnsAsync(product);

            // Act
            var result = await _repository.GetByIdAsync("872522");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Product1", result.Name);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProductAndReturnResponse()
        {
            // Arrange
            var product = new Product { Name = "New Product" };
            _mockSet.Setup(m => m.Add(It.IsAny<Product>()));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var response = await _repository.AddAsync(product);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.ProductId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Product { ProductId = "1", Name = "Updated Product" };

            var mockEntityEntry = new Mock<EntityEntry<Product>>();
            mockEntityEntry.SetupProperty(e => e.State, EntityState.Modified);

            _mockContext.Setup(c => c.Entry(product)).Returns(mockEntityEntry.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _repository.UpdateAsync(product);

            // Assert
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { ProductId = "872522", Name = "Product1" };
            _mockSet.Setup(m => m.FindAsync("872522")).ReturnsAsync(product);
            _mockSet.Setup(m => m.Remove(product));
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            await _repository.DeleteAsync("872522");

            // Assert
            _mockSet.Verify(m => m.Remove(product), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenProductExists()
        {
            // Arrange
            _mockSet.Setup(m => m.AnyAsync(p => p.ProductId == "1", default)).ReturnsAsync(true);

            // Act
            var result = await _repository.ExistsAsync("1");

            // Assert
            Assert.True(result);
        }



    }
}
