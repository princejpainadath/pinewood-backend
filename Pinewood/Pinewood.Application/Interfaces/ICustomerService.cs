using Pinewood.Shared.Dtos;

namespace Pinewood.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>?> GetCustomersAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(Guid id);
        Task<CustomerDto?> AddCustomerAsync(AddCustomerDto customer);
        Task<CustomerDto?> UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
        Task<bool> DeleteCustomerAsync(Guid id);
    }
}
