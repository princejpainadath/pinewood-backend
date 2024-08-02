using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pinewood.Domain.Entities;
using Pinewood.Domain.Interfaces;
using Pinewood.Infrastructure.Context;

namespace Pinewood.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly PinewoodDbContext _dbContext;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(PinewoodDbContext dbContext, ILogger<CustomerRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all active customers.
        /// </summary>
        /// <returns>A list of active customers.</returns>
        public async Task<List<Customer>?> GetCustomersAsync()
        {
            try
            {
                return await _dbContext.Customer
                    .Where(item => item.Status == Domain.Enums.CustomerStatus.Active)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier if they are active.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <returns>The customer details if found.</returns>
        public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        {
            try
            {
                var customer = await _dbContext.Customer.FindAsync(id);
                if (customer != null && customer.Status == Domain.Enums.CustomerStatus.Active)
                {
                    return customer;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Adds a new customer.
        /// </summary>
        /// <param name="customer">The details of the customer to be added.</param>
        /// <returns>The details of the newly added customer.</returns>
        public async Task<Customer?> AddCustomerAsync(Customer customer)
        {
            try
            {
                await _dbContext.Customer.AddAsync(customer);
                await _dbContext.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Updates an existing customer's details.
        /// </summary>
        /// <param name="customer">The updated details of the customer.</param>
        /// <returns>The details of the updated customer.</returns>
        public async Task<Customer?> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                _dbContext.Customer.Update(customer);
                await _dbContext.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Check specified email address exists.
        /// </summary>
        /// <param name="email">The email address to check for existence.</param>
        /// <returns>Returns true if the email exists, false it does not, and null if an error occurs.</returns>
        public async Task<bool?> EmailExistsAsync(string email)
        {
            try
            {
                return await _dbContext.Customer.AnyAsync(item => item.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
