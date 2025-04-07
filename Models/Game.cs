using System.ComponentModel.DataAnnotations;

namespace Onboardle.Models
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateOnly? GameDate { get; set; }
        public Guid PhotoId { get; set; }
        public virtual Photo Photo { get; set; } = null!;
    }
}