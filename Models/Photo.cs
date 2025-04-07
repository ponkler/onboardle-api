using System.ComponentModel.DataAnnotations;

namespace Onboardle.Models
{
    public class Photo
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Path { get; set; } = string.Empty;
        public string Track { get; set; } = string.Empty;
        public string Driver { get; set; } = string.Empty;
        public int Year { get; set; }

        public virtual Game? Game { get; set; }
    }
}
