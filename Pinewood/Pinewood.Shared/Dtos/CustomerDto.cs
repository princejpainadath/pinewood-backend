namespace Pinewood.Shared.Dtos
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; } 
        public required string LastName { get; set; }
        public required string Email { get; set; } 
    }
}
