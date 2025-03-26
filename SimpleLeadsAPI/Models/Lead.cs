using System.ComponentModel.DataAnnotations;

namespace SimpleLeadsAPI.Models
{
    public class Lead
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }
      
        public string? ContactNumber { get; set; }

        public string? CurrentlyInsured { get; set; }

        public string? Insurer {  get; set; }

        [MaxLength(12)]
        public string? OtherInsurer {  get; set; }
    }
}
