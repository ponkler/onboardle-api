using System.ComponentModel.DataAnnotations;

namespace Onboardle.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
