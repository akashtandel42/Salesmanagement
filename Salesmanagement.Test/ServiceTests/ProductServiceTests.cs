namespace Salesmanagement.Test.ServiceTests
{
    using global::Salesmanagement.Models;
    using global::Salesmanagement.Repositories;
    using global::Salesmanagement.Services;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Xunit;

    namespace Salesmanagement.Tests
    {
        public class ProductServiceTests
        {
            private readonly ProductService _productService;
            private readonly Mock<IProductRepository> _productRepositoryMock;

            public ProductServiceTests()
            {
                _productRepositoryMock = new Mock<IProductRepository>();
                _productService = new ProductService(_productRepositoryMock.Object);
            }

            [Fact]
            public async Task CreateProductAsync_AddsProductToRepository()
            {
                // Arrange
                var product = new Product { Id = 1, Name = "Product1", Price = 100, Description = "Description1" };
                _productRepositoryMock.Setup(repo => repo.AddAsync(product)).Returns(Task.CompletedTask);

                // Act
                var result = await _productService.CreateProductAsync(product);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(product.Id, result.Id);
                _productRepositoryMock.Verify(repo => repo.AddAsync(product), Times.Once);
            }

            [Fact]
            public async Task GetProductByIdAsync_ReturnsProduct_WhenProductExists()
            {
                // Arrange
                var productId = 1;
                var product = new Product { Id = productId, Name = "Product1", Price = 100, Description = "Description1" };
                _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

                // Act
                var result = await _productService.GetProductByIdAsync(productId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(productId, result.Id);
            }

            [Fact]
            public async Task GetProductByIdAsync_ReturnsNull_WhenProductDoesNotExist()
            {
                // Arrange
                var productId = 1;
                _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product)null);

                // Act
                var result = await _productService.GetProductByIdAsync(productId);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task UpdateProductAsync_UpdatesProductInRepository()
            {
                // Arrange
                var productId = 1;
                var existingProduct = new Product { Id = productId, Name = "OldName", Price = 100, Description = "OldDescription" };
                var updatedProduct = new Product { Id = productId, Name = "NewName", Price = 200, Description = "NewDescription" };

                _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(existingProduct);
                _productRepositoryMock.Setup(repo => repo.UpdateAsync(existingProduct)).Returns(Task.CompletedTask);

                // Act
                var result = await _productService.UpdateProductAsync(productId, updatedProduct);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(updatedProduct.Name, result.Name);
                Assert.Equal(updatedProduct.Price, result.Price);
                Assert.Equal(updatedProduct.Description, result.Description);
                _productRepositoryMock.Verify(repo => repo.UpdateAsync(existingProduct), Times.Once);
            }

            [Fact]
            public async Task UpdateProductAsync_ReturnsNull_WhenProductDoesNotExist()
            {
                // Arrange
                var productId = 1;
                var updatedProduct = new Product { Id = productId, Name = "NewName", Price = 200, Description = "NewDescription" };

                _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product)null);

                // Act
                var result = await _productService.UpdateProductAsync(productId, updatedProduct);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task DeleteProductAsync_DeletesProductFromRepository()
            {
                // Arrange
                var productId = 1;
                _productRepositoryMock.Setup(repo => repo.DeleteAsync(productId)).Returns(Task.CompletedTask);

                // Act
                await _productService.DeleteProductAsync(productId);

                // Assert
                _productRepositoryMock.Verify(repo => repo.DeleteAsync(productId), Times.Once);
            }

            [Fact]
            public async Task GetAllProductsAsync_ReturnsAllProducts()
            {
                // Arrange
                var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Price = 100, Description = "Description1" },
                new Product { Id = 2, Name = "Product2", Price = 200, Description = "Description2" }
            };
                _productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

                // Act
                var result = await _productService.GetAllProductsAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
            }
        }
    }

}
