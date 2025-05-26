namespace Domain.DTOs
{
    public record PlatformCreateDto
    {
        public string? Name { get; set; }
        public string? Publisher { get; set; }
        public decimal Cost { get; set; }
    }
}
