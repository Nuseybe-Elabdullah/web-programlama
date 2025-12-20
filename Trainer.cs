using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    /// <summary>
    /// Represents a trainer at the gym
    /// يمثل مدرب في الصالة الرياضية
    /// </summary>
    public class Trainer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Trainer name is required")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(100)]
        [Display(Name = "Specialization")]
        public string Specialization { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Gym")]
        public int GymId { get; set; }

        // Navigation properties
        [ForeignKey("GymId")]
        public virtual Gym? Gym { get; set; }
        
        public virtual ICollection<TrainerService> TrainerServices { get; set; } = new List<TrainerService>();
        public virtual ICollection<TrainerAvailability> TrainerAvailabilities { get; set; } = new List<TrainerAvailability>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
