using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pinewood.Api.Controllers;
using Pinewood.Application.Interfaces;
using Pinewood.Shared.Dtos;
using Xunit;

namespace Pinewood.Tests.UnitTests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerService> _mockCustomerService;
        private readonly Mock<ILogger<CustomerController>> _mockLogger;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _mockLogger = new Mock<ILogger<CustomerController>>();
            _controller = new CustomerController(_mockCustomerService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetCustomers_ReturnsOk_WithCustomers()
        {
            // Arrange
            var customers = new List<CustomerDto>
        {
            new CustomerDto { Id = Guid.NewGuid(), FirstName = "Emma", LastName = "Watson", Email = "test@email.com" },
            new CustomerDto { Id = Guid.NewGuid(), FirstName = "Watson", LastName = "Emma", Email = "test2@email.com" }
        };

            _mockCustomerService
                .Setup(service => service.GetCustomersAsync())
                .ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomers = Assert.IsType<List<CustomerDto>>(actionResult.Value);
            Assert.Equal(customers.Count, returnedCustomers.Count);
        }

        [Fact]
        public async Task GetCustomer_ReturnsOk_WithCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customerDto = new CustomerDto { Id = customerId, FirstName = "Emma", LastName = "Watson", Email = "test@email.com" };
            _mockCustomerService
                .Setup(service => service.GetCustomerByIdAsync(customerId))
                .ReturnsAsync(customerDto);

            // Act
            var result = await _controller.GetCustomer(customerId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomer = Assert.IsType<CustomerDto>(actionResult.Value);
            Assert.Equal(customerId, returnedCustomer.Id);
        }

        [Fact]
        public async Task GetCustomer_ReturnsNotFound_WhenCustomerIsNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _mockCustomerService
                .Setup(service => service.GetCustomerByIdAsync(customerId))
                .ReturnsAsync((CustomerDto?)null);

            // Act
            var result = await _controller.GetCustomer(customerId);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddCustomer_ReturnsOk_WithCustomer()
        {
            // Arrange
            var addCustomerDto = new AddCustomerDto { FirstName = "Emma", LastName = "Watson", Email = "test@email.com" };
            var customerDto = new CustomerDto { Id = Guid.NewGuid(), FirstName = "Emma", LastName = "Watson", Email = "test@email.com" };

            _mockCustomerService
                .Setup(service => service.AddCustomerAsync(addCustomerDto))
                .ReturnsAsync(customerDto);

            // Act
            var result = await _controller.AddCustomer(addCustomerDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<CustomerDto>(actionResult.Value);
            Assert.Equal(customerDto, actionResult.Value);
        }

        [Fact]
        public async Task AddCustomer_ReturnsBadRequest_WithEmailExistsMessage()
        {
            // Arrange
            var addCustomerDto = new AddCustomerDto { FirstName = "Emma", LastName = "Watson", Email = "test@email.com" };

            _mockCustomerService
                .Setup(service => service.AddCustomerAsync(addCustomerDto))
                .ReturnsAsync((CustomerDto?)null);

            // Act
            var result = await _controller.AddCustomer(addCustomerDto);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);
            Assert.Equal("Email already exists.", actionResult.Value);
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsOk_WithCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var updateCustomerDto = new UpdateCustomerDto { Id = customerId, FirstName = "Emma", LastName = "Watson" };
            var customerDto = new CustomerDto { Id = customerId, FirstName = "Emma", LastName = "Watson", Email = "test@email.com" };

            _mockCustomerService
                .Setup(service => service.UpdateCustomerAsync(updateCustomerDto))
                .ReturnsAsync(customerDto);

            // Act
            var result = await _controller.UpdateCustomer(updateCustomerDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<CustomerDto>(actionResult.Value);
            Assert.Equal(customerDto, actionResult.Value);
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsStatus500()
        {
            // Arrange
            var updateCustomerDto = new UpdateCustomerDto { Id = Guid.NewGuid(), FirstName = "Emma", LastName = "Watson" };

            _mockCustomerService
                .Setup(service => service.UpdateCustomerAsync(updateCustomerDto))
                .ReturnsAsync((CustomerDto?)null);

            // Act
            var result = await _controller.UpdateCustomer(updateCustomerDto);

            // Assert
            var actionResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, actionResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsOk_WhenCustomerIsDeleted()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _mockCustomerService
                .Setup(service => service.DeleteCustomerAsync(customerId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCustomer(customerId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNotFound_WhenCustomerIsNotDeleted()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _mockCustomerService
                .Setup(service => service.DeleteCustomerAsync(customerId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCustomer(customerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
