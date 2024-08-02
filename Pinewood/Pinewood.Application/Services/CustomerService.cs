using AutoMapper;
using Microsoft.Extensions.Logging;
using Pinewood.Application.Interfaces;
using Pinewood.Domain.Entities;
using Pinewood.Domain.Interfaces;
using Pinewood.Shared.Dtos;

namespace Pinewood.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all active customers.
        /// </summary>
        /// <returns>A list of active customers.</returns>
        public async Task<List<CustomerDto>?> GetCustomersAsync()
        {
            try
            {
                var customers = await _customerRepository.GetCustomersAsync();
                return _mapper.Map<List<CustomerDto>>(customers);
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
        public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                return _mapper.Map<CustomerDto>(customer);
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
        /// <param name="addCustomerDto">The details of the customer to be added.</param>
        /// <returns>The details of the newly added customer.</returns>
        public async Task<CustomerDto?> AddCustomerAsync(AddCustomerDto addCustomerDto)
        {
            try
            {
                // Check if the email already exists
                var isEmailExists = await _customerRepository.EmailExistsAsync(addCustomerDto.Email);
                if (isEmailExists == null || isEmailExists == true)
                {
                    return null;
                }

                var customer = _mapper.Map<Customer>(addCustomerDto);
                customer.Id = Guid.NewGuid();
                customer.CreatedOn = DateTime.UtcNow;
                customer.LastUpdatedOn = DateTime.UtcNow;
                customer.Status = Domain.Enums.CustomerStatus.Active;
                await _customerRepository.AddCustomerAsync(customer);
                return _mapper.Map<CustomerDto>(customer);
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
        /// <param name="updateCustomerDto">The updated details of the customer.</param>
        /// <returns>The details of the updated customer.</returns>
        public async Task<CustomerDto?> UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(updateCustomerDto.Id);
                if (customer == null)
                {
                    return null;
                }

                _mapper.Map(updateCustomerDto, customer);
                customer.LastUpdatedOn = DateTime.UtcNow;
                await _customerRepository.UpdateCustomerAsync(customer);
                return _mapper.Map<CustomerDto>(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Soft deletes a customer by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to be deleted.</param>
        /// <returns> If it soft deleted it returns true otherwise false.</returns>
        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return false;
                }

                customer.LastUpdatedOn = DateTime.UtcNow;
                customer.Status = Domain.Enums.CustomerStatus.Deleted;
                await _customerRepository.UpdateCustomerAsync(customer);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
