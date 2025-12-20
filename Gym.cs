using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Models
{
    /// <summary>
    /// Represents a gym/fitness center location
    /// يمثل موقع الصالة الرياضية
    /// </summary>
    public class Gym
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Gym name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Gym Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Working hours are required")]
        [StringLength(100)]
        [Display(Name = "Working Hours")]
        public string WorkingHours { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
