namespace SimpleLeadsAPI.Models
{
    public class Lead
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? CellNumber { get; set; }
    }
}
