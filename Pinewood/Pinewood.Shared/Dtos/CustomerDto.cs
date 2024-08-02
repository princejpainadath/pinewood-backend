namespace Pinewood.Shared.Dtos
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; } 
        public required string LastName { get; set; }
        public required string Email { get; set; } 
        //public DateTime CreatedOn { get; set; }
        //public DateTime LastUpdatedOn { get; set; }
        //public int Status { get; set; }
    }
}
