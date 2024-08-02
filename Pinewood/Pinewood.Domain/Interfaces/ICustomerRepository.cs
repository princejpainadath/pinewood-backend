using Pinewood.Domain.Entities;

namespace Pinewood.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>?> GetCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<Customer?> AddCustomerAsync(Customer customer);
        Task<Customer?> UpdateCustomerAsync(Customer customer);
        Task<bool?> EmailExistsAsync(string email);
    }
}
