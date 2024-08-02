using System.ComponentModel.DataAnnotations;

namespace Pinewood.Shared.Dtos
{
    public class AddCustomerDto
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
