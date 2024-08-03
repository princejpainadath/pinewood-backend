using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Pinewood.Application.Services;
using Pinewood.Domain.Entities;
using Pinewood.Domain.Interfaces;
using Pinewood.Shared.Dtos;
using Xunit;

namespace Pinewood.Tests.UnitTests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly IMapper _mapper;
        private readonly CustomerService _customerService;
        private readonly Mock<ILogger<CustomerService>> _mockLogger;

        public CustomerServiceTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockLogger = new Mock<ILogger<CustomerService>>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDto>();
                cfg.CreateMap<AddCustomerDto, Customer>();
                cfg.CreateMap<UpdateCustomerDto, Customer>();
            });

            _mapper = mapperConfig.CreateMapper();
            _customerService = new CustomerService(_mockCustomerRepository.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task GetCustomersAsync_ReturnsWithCustomers()
        {
            // Arrange
            var customers = new List<Customer>
        {
            new Customer { Id = Guid.NewGuid(), FirstName = "Emma", LastName = "Watson", Email = "test@email.com" },
            new Customer { Id = Guid.NewGuid(), FirstName = "Watson", LastName = "Emma", Email = "test2@email.com" }
        };

            _mockCustomerRepository
                .Setup(repository => repository.GetCustomersAsync())
                .ReturnsAsync(customers);

            // Act
            var result = await _customerService.GetCustomersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customers.Count, result.Count);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsWithCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer { Id = customerId, FirstName = "Emma", LastName = "Watson", Email = "test@email.com" };

            _mockCustomerRepository
                .Setup(repository => repository.GetCustomerByIdAsync(customerId))
                .ReturnsAsync(customer);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(customerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerId, result.Id);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsNull_WhenCustomerIsNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _mockCustomerRepository
                .Setup(repository => repository.GetCustomerByIdAsync(customerId))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerService.GetCustomerByIdAsync(customerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddCustomerAsync_ReturnsWithCustomer()
        {
            // Arrange
            var addCustomerDto = new AddCustomerDto
            {
                FirstName = "Emma",
                LastName = "Watson",
                Email = "test@email.com"
            };

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Emma",
                LastName = "Watson",
                Email = "test@email.com"
            };

            _mockCustomerRepository
              .Setup(repository => repository.EmailExistsAsync(It.IsAny<string>()))
              .ReturnsAsync(false);

            _mockCustomerRepository
                .Setup(repository => repository.AddCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync((Customer item) =>
                {
                    item.Id = customer.Id;
                    return item;
                });

            // Act
            var result = await _customerService.AddCustomerAsync(addCustomerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, customer.Id);
            Assert.Equal("Emma", result.FirstName);
            Assert.Equal("Watson", result.LastName);
            Assert.Equal("test@email.com", result.Email);
        }

        [Fact]
        public async Task AddCustomerAsync_ReturnsNull_WhenEmailAlreadyExists()
        {
            // Arrange
            var addCustomerDto = new AddCustomerDto
            {
                FirstName = "Emma",
                LastName = "Watson",
                Email = "test@email.com"
            };

            _mockCustomerRepository
              .Setup(repository => repository.EmailExistsAsync(It.IsAny<string>()))
              .ReturnsAsync(true);

            // Act
            var result = await _customerService.AddCustomerAsync(addCustomerDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ReturnsWithUpdatedCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var updateCustomerDto = new UpdateCustomerDto
            {
                Id = customerId,
                FirstName = "Watson",
                LastName = "Emma"
            };

            var customer = new Customer
            {
                Id = customerId,
                FirstName = "Emma",
                LastName = "Watson",
                Email = "test@email.com"
            };

            _mockCustomerRepository
               .Setup(repository => repository.GetCustomerByIdAsync(customerId))
               .ReturnsAsync(customer);

            _mockCustomerRepository
                .Setup(repository => repository.UpdateCustomerAsync(It.IsAny<Customer>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _customerService.UpdateCustomerAsync(updateCustomerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, customer.Id);
            Assert.Equal("Watson", result.FirstName);
            Assert.Equal("Emma", result.LastName);
            Assert.Equal("test@email.com", result.Email);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ReturnsNull_WhenCustomerIsNotFound()
        {
            var customerId = Guid.NewGuid();
            // Arrange
            var updateCustomerDto = new UpdateCustomerDto
            {
                Id = customerId,
                FirstName = "Watson",
                LastName = "Emma"
            };

            _mockCustomerRepository
               .Setup(repository => repository.GetCustomerByIdAsync(customerId))
               .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerService.UpdateCustomerAsync(updateCustomerDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ReturnsTrue_WhenCustomerIsDeleted()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                Id = customerId,
                FirstName = "Emma",
                LastName = "Watson",
                Email = "test@email.com"
            };

            _mockCustomerRepository
             .Setup(repository => repository.GetCustomerByIdAsync(customerId))
             .ReturnsAsync(customer);

            // Act
            var result = await _customerService.DeleteCustomerAsync(customerId);

            // Assert
            Assert.True(result);
            _mockCustomerRepository.Verify(repository => repository.UpdateCustomerAsync(customer), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ReturnsFalse_WhenCustomerIsNotDeleted()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _mockCustomerRepository
             .Setup(repository => repository.GetCustomerByIdAsync(customerId))
             .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerService.DeleteCustomerAsync(customerId);

            // Assert
            Assert.False(result);
        }
    }
}
