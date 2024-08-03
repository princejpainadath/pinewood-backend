using System.ComponentModel.DataAnnotations;

namespace Pinewood.Shared.Dtos
{
    public class UpdateCustomerDto
    {
        [Required]
        public required Guid Id { get; set; }
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
    }
}
