using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public record CommandCreateDto
    {
        public string? HowTo { get; set; }
        public string? CommandLine { get; set; }
    }
}
