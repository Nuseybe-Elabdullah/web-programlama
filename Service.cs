using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    /// <summary>
    /// Represents a service offered at the gym
    /// يمثل خدمة مقدمة في الصالة الرياضية
    /// </summary>
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Service name is required")]
        [StringLength(100)]
        [Display(Name = "Service Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Duration is required")]
        [Range(15, 240, ErrorMessage = "Duration must be between 15 and 240 minutes")]
        [Display(Name = "Duration (minutes)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000, ErrorMessage = "Price must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Gym")]
        public int GymId { get; set; }

        [StringLength(500)]
        [Display(Name = "Fotoğraf")]
        public string? ImagePath { get; set; }

        // Navigation properties
        [ForeignKey("GymId")]
        public virtual Gym? Gym { get; set; }
        
        public virtual ICollection<TrainerService> TrainerServices { get; set; } = new List<TrainerService>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
