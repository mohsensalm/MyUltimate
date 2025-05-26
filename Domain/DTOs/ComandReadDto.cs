using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public record ComandReadDto
    {
        public int Id { get; set; }
        public string HowTo { get; set; } = string.Empty;
        public string CommandLine { get; set; } = string.Empty;
        public int PlatformId { get; set; }

    }
}
