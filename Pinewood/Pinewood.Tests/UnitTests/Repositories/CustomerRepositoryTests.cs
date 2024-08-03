using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Pinewood.Domain.Entities;
using Pinewood.Infrastructure.Context;
using Pinewood.Infrastructure.Repositories;
using Xunit;

namespace Pinewood.Tests.UnitTests.Repositories
{
    public class CustomerRepositoryTests
    {
        private readonly DbContextOptions<PinewoodDbContext> _options;
        private readonly Mock<ILogger<CustomerRepository>> _mockLogger;

        public CustomerRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<PinewoodDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _mockLogger = new Mock<ILogger<CustomerRepository>>();
        }

        [Fact]
        public async Task GetCustomersAsync_ReturnsWithCustomers()
        {
            using (var context = new PinewoodDbContext(_options))
            {
                context.Customer.AddRange(new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), FirstName = "Kevin", LastName = "John", Email = "test@email.com", Status = Domain.Enums.CustomerStatus.Active },
                new Customer { Id = Guid.NewGuid(), FirstName = "John", LastName = "Kevin", Email="test2@email.com", Status = Domain.Enums.CustomerStatus.Active },
                new Customer { Id = Guid.NewGuid(), FirstName = "Emma", LastName = "Watson", Email="test3@email.com", Status = Domain.Enums.CustomerStatus.Deleted }
            });
                context.SaveChanges();
            }

            using (var context = new PinewoodDbContext(_options))
            {
                var repository = new CustomerRepository(context, _mockLogger.Object);
                var result = await repository.GetCustomersAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
            }
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsWithCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            using (var context = new PinewoodDbContext(_options))
            {
                context.Customer.Add(new Customer { Id = customerId, FirstName = "Emma", LastName = "Watson", Email = "test@email.com", Status = Domain.Enums.CustomerStatus.Active });
                context.SaveChanges();
            }

            using (var context = new PinewoodDbContext(_options))
            {
                var repository = new CustomerRepository(context, _mockLogger.Object);
                var result = await repository.GetCustomerByIdAsync(customerId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(customerId, result.Id);
            }
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsNull_WhenCustomerIsNotFound()
        {
            using (var context = new PinewoodDbContext(_options))
            {
                var repository = new CustomerRepository(context, _mockLogger.Object);
                var result = await repository.GetCustomerByIdAsync(Guid.NewGuid());

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddCustomerAsync_ReturnsWithCustomer()
        {
            using (var context = new PinewoodDbContext(_options))
            {
                var repository = new CustomerRepository(context, _mockLogger.Object);
                var customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Emma",
                    LastName = "Watson",
                    Email = "test@email.com",
                    CreatedOn = DateTime.UtcNow,
                    LastUpdatedOn = DateTime.UtcNow,
                    Status = Domain.Enums.CustomerStatus.Active
                };

                await repository.AddCustomerAsync(customer);
                var result = await context.Customer.FirstOrDefaultAsync(item => item.Id == customer.Id);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(customer.FirstName, result.FirstName);
                Assert.Equal(customer.LastName, result.LastName);
            }
        }

        [Fact]
        public async Task UpdateCustomerAsync_ReturnsWithUpdatedCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            using (var context = new PinewoodDbContext(_options))
            {
                context.Customer.Add(new Customer
                {
                    Id = customerId,
                    FirstName = "Emma",
                    LastName = "Watson",
                    Status = Domain.Enums.CustomerStatus.Active
                });
                context.SaveChanges();
            }

            using (var context = new PinewoodDbContext(_options))
            {
                var repository = new CustomerRepository(context, _mockLogger.Object);
                var customer = await repository.GetCustomerByIdAsync(customerId);
                if (customer != null)
                {
                    customer.FirstName = "Kevin";
                    await repository.UpdateCustomerAsync(customer);
                }
                var result = await context.Customer.FirstOrDefaultAsync(c => c.Id == customerId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Kevin", result.FirstName);
            }
        }

        [Fact]
        public async Task EmailExistsAsync_ReturnsTrue_WhenEmailExists()
        {
            // Arrange
            var email = "test@email.com";
            using (var context = new PinewoodDbContext(_options))
            {
                context.Customer.Add(new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Emma",
                    LastName = "Watson",
                    Email = email,
                    Status = Domain.Enums.CustomerStatus.Active
                });
                context.SaveChanges();
            }

            using (var context = new PinewoodDbContext(_options))
            {
                var repository = new CustomerRepository(context, _mockLogger.Object);
                var result = await repository.EmailExistsAsync(email);

                // Assert
                Assert.True(result.HasValue);
                Assert.True(result.Value);
            }
        }

        [Fact]
        public async Task EmailExistsAsync_ReturnsFalse_WhenEmailNotExist()
        {
            // Arrange
            var email = "test@email.com";

            using (var context = new PinewoodDbContext(_options))
            {
                var repository = new CustomerRepository(context, _mockLogger.Object);
                var result = await repository.EmailExistsAsync(email);

                // Assert
                Assert.True(result.HasValue);
                Assert.False(result.Value);
            }
        }
    }
}
