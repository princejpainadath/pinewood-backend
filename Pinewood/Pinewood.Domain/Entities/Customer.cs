using Pinewood.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Pinewood.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime LastUpdatedOn { get; set; }
        [Required]
        public CustomerStatus Status { get; set; }
    }
}
