using System.ComponentModel.DataAnnotations;

namespace Leads.External.Models
{
    public class CreateLeadDto
    {
        public string? FullName { get; set; }

        public string? ContactNumber { get; set; }

        public string? CurrentlyInsured { get; set; }

        public string? Insurer { get; set; }

        public string? OtherInsurer { get; set; }
    }
}
