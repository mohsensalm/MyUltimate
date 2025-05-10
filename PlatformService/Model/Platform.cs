using System.ComponentModel.DataAnnotations;

namespace PlatformService.Model
{
    public class Platform
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Publisher { get; set; }
        [Required]
        public decimal Cost  { get; set; }
    }
}
