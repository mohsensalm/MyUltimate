using System.ComponentModel.DataAnnotations;

namespace PlatformService.DTOs
{
    public record PlatformReadDto
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Publisher { get; set; }
        public decimal Cost { get; set; }
    }
}
