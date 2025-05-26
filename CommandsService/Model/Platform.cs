using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace CommandsService.Model
{
    public class Platform
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int ExternalID { get; set; } = 0;
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Publisher { get; set; } = string.Empty;
        public string Cost { get; set; } = string.Empty;
        public ICollection<Command> Commands { get; set; } = [];
    }
}
