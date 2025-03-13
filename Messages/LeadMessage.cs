using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class LeadMessage
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }

        public string? ContactNumber { get; set; }

        public string? CurrentlyInsured { get; set; }

        public string? Insurer { get; set; }

        public string? OtherInsurer { get; set; }
    }
}
