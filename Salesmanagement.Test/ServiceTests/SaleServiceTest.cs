namespace Salesmanagement.Test.ServiceTests
{
    using global::Salesmanagement.Models;
    using global::Salesmanagement.Repositories;
    using global::Salesmanagement.Services;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    namespace Salesmanagement.Tests
    {
        public class SaleServiceTests
        {
            private readonly SalesService _saleService;
            private readonly Mock<ISaleRepository> _saleRepositoryMock;

            public SaleServiceTests()
            {
                _saleRepositoryMock = new Mock<ISaleRepository>();
                _saleService = new SalesService(_saleRepositoryMock.Object);
            }

            [Fact]
            public async Task CreateSaleAsync_AddsSaleToRepository()
            {
                // Arrange
                var sale = new Sale { Id = 1, CustomerName = "John Doe", Amount = 100, SaleDate = DateTime.Now, ProductId = 1, RegionId = 1 };
                _saleRepositoryMock.Setup(repo => repo.AddAsync(sale)).Returns(Task.CompletedTask);

                // Act
                var result = await _saleService.CreateSaleAsync(sale);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(sale.Id, result.Id);
                _saleRepositoryMock.Verify(repo => repo.AddAsync(sale), Times.Once);
            }

            [Fact]
            public async Task GetSaleByIdAsync_ReturnsSale_WhenSaleExists()
            {
                // Arrange
                var saleId = 1;
                var sale = new Sale { Id = saleId, CustomerName = "John Doe", Amount = 100, SaleDate = DateTime.Now, ProductId = 1, RegionId = 1 };
                _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(saleId)).ReturnsAsync(sale);

                // Act
                var result = await _saleService.GetSaleByIdAsync(saleId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(saleId, result.Id);
            }

            [Fact]
            public async Task GetSaleByIdAsync_ReturnsNull_WhenSaleDoesNotExist()
            {
                // Arrange
                var saleId = 1;
                _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(saleId)).ReturnsAsync((Sale)null);

                // Act
                var result = await _saleService.GetSaleByIdAsync(saleId);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task UpdateSaleAsync_UpdatesSaleInRepository()
            {
                // Arrange
                var saleId = 1;
                var existingSale = new Sale { Id = saleId, CustomerName = "Old Name", Amount = 100, SaleDate = DateTime.Now, ProductId = 1, RegionId = 1 };
                var updatedSale = new Sale { Id = saleId, CustomerName = "New Name", Amount = 200, SaleDate = DateTime.Now, ProductId = 1, RegionId = 1 };

                _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(saleId)).ReturnsAsync(existingSale);
                _saleRepositoryMock.Setup(repo => repo.UpdateAsync(existingSale)).Returns(Task.CompletedTask);

                // Act
                var result = await _saleService.UpdateSaleAsync(saleId, updatedSale);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(updatedSale.CustomerName, result.CustomerName);
                Assert.Equal(updatedSale.Amount, result.Amount);
                _saleRepositoryMock.Verify(repo => repo.UpdateAsync(existingSale), Times.Once);
            }

            [Fact]
            public async Task UpdateSaleAsync_ReturnsNull_WhenSaleDoesNotExist()
            {
                // Arrange
                var saleId = 1;
                var updatedSale = new Sale { Id = saleId, CustomerName = "New Name", Amount = 200, SaleDate = DateTime.Now, ProductId = 1, RegionId = 1 };

                _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(saleId)).ReturnsAsync((Sale)null);

                // Act
                var result = await _saleService.UpdateSaleAsync(saleId, updatedSale);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task DeleteSaleAsync_DeletesSaleFromRepository()
            {
                // Arrange
                var saleId = 1;
                _saleRepositoryMock.Setup(repo => repo.DeleteAsync(saleId)).Returns(Task.CompletedTask);

                // Act
                await _saleService.DeleteSaleAsync(saleId);

                // Assert
                _saleRepositoryMock.Verify(repo => repo.DeleteAsync(saleId), Times.Once);
            }

            [Fact]
            public async Task GetAllSalesAsync_ReturnsAllSales()
            {
                // Arrange
                var sales = new List<Sale>
            {
                new Sale { Id = 1, CustomerName = "John Doe", Amount = 100, SaleDate = DateTime.Now, ProductId = 1, RegionId = 1 },
                new Sale { Id = 2, CustomerName = "Jane Doe", Amount = 200, SaleDate = DateTime.Now, ProductId = 2, RegionId = 2 }
            };
                _saleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(sales);

                // Act
                var result = await _saleService.GetAllSalesAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
            }

            [Fact]
            public async Task CalculateTotalSalesAsync_ReturnsCorrectTotalSales()
            {
                // Arrange
                var startDate = new DateTime(2023, 1, 1);
                var endDate = new DateTime(2023, 12, 31);
                var sales = new List<Sale>
            {
                new Sale { Id = 1, CustomerName = "John Doe", Amount = 100, SaleDate = new DateTime(2023, 6, 1), ProductId = 1, RegionId = 1 },
                new Sale { Id = 2, CustomerName = "Jane Doe", Amount = 200, SaleDate = new DateTime(2023, 7, 1), ProductId = 2, RegionId = 2 }
            };
                _saleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(sales);

                // Act
                var result = await _saleService.CalculateTotalSalesAsync(startDate, endDate);

                // Assert
                Assert.Equal(300, result);
            }

            [Fact]
            public async Task GetSalesTrendsAsync_ReturnsCorrectSalesTrends()
            {
                // Arrange
                var sales = new List<Sale>
            {
                new Sale { Id = 1, CustomerName = "John Doe", Amount = 100, SaleDate = new DateTime(2023, 6, 1), ProductId = 1, RegionId = 1 },
                new Sale { Id = 2, CustomerName = "Jane Doe", Amount = 200, SaleDate = new DateTime(2023, 6, 7), ProductId = 2, RegionId = 2 },
                new Sale { Id = 3, CustomerName = "Jim Doe", Amount = 150, SaleDate = new DateTime(2023, 6, 15), ProductId = 3, RegionId = 3 }
            };
                _saleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(sales);

                // Act
                var result = await _saleService.GetSalesTrendsAsync("daily");

                // Assert
                Assert.NotNull(result);
                Assert.Equal(3, result.Count());
            }

            [Fact]
            public async Task GetTopSellingProductsAsync_ReturnsTopSellingProducts()
            {
                // Arrange
                var startDate = new DateTime(2023, 1, 1);
                var endDate = new DateTime(2023, 12, 31);
                var sales = new List<Sale>
            {
                new Sale { Id = 1, CustomerName = "John Doe", Amount = 100, SaleDate = new DateTime(2023, 6, 1), ProductId = 1, RegionId = 1 },
                new Sale { Id = 2, CustomerName = "Jane Doe", Amount = 200, SaleDate = new DateTime(2023, 7, 1), ProductId = 2, RegionId = 2 },
                new Sale { Id = 3, CustomerName = "Jim Doe", Amount = 150, SaleDate = new DateTime(2023, 6, 15), ProductId = 1, RegionId = 3 }
            };
                _saleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(sales);

                // Act
                var result = await _saleService.GetTopSellingProductsAsync(startDate, endDate, 1);

                // Assert
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Equal(1, result[0].Id);
            }

            [Fact]
            public async Task GetSalesByRegionAsync_ReturnsSalesByRegion()
            {
                // Arrange
                var sales = new List<Sale>
            {
                new Sale { Id = 1, CustomerName = "John Doe", Amount = 100, SaleDate = new DateTime(2023, 6, 1), ProductId = 1, RegionId = 1 },
                new Sale { Id = 2, CustomerName = "Jane Doe", Amount = 200, SaleDate = new DateTime(2023, 7, 1), ProductId = 2, RegionId = 1 },
                new Sale { Id = 3, CustomerName = "Jim Doe", Amount = 150, SaleDate = new DateTime(2023, 6, 15), ProductId = 3, RegionId = 2 }
            };
                _saleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(sales);

                // Act
                var result = await _saleService.GetSalesByRegionAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.Equal(300, result["1"]);
                Assert.Equal(150, result["2"]);
            }
        }
    }

}