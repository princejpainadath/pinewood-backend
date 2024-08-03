using Microsoft.AspNetCore.Mvc;
using Pinewood.Application.Interfaces;
using Pinewood.Shared.Dtos;

namespace Pinewood.Api.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all active customers.
        /// </summary>
        /// <returns>A list of active customers.</returns>
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = await _customerService.GetCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier if they are active.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <returns>The customer details if found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            try
            {
                var customerDto = await _customerService.GetCustomerByIdAsync(id);
                if (customerDto == null)
                {
                    return NotFound();
                }
                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Adds a new customer.
        /// </summary>
        /// <param name="addCustomerDto">The details of the customer to be added.</param>
        /// <returns>The details of the newly added customer.</returns>
        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] AddCustomerDto addCustomerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var customerDto = await _customerService.AddCustomerAsync(addCustomerDto);
                if (customerDto == null)
                {
                    return BadRequest("Email already exists.");
                }
                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Updates an existing customer's details.
        /// </summary>
        /// <param name="updateCustomerDto">The updated details of the customer.</param>
        /// <returns>The details of the updated customer.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerDto updateCustomerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var customerDto = await _customerService.UpdateCustomerAsync(updateCustomerDto);
                if (customerDto == null)
                {
                    return StatusCode(500);
                }
                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Soft deletes a customer by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to be deleted.</param>
        /// <returns> If it soft deleted it returns true otherwise false.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            try
            {
                var result = await _customerService.DeleteCustomerAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
