﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public record PlatformPublishedDto
    {
        public int Id { get; init; }
        public string? Name { get; init; } = string.Empty;
        public string? Event { get; set; } = string.Empty;
    }
}
