using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    /// <summary>
    /// Represents an appointment booking
    /// يمثل حجز موعد
    /// </summary>
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Member")]
        public string MemberId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }

        [Required]
        [Display(Name = "Service")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Display(Name = "Status")]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("MemberId")]
        public virtual ApplicationUser? Member { get; set; }

        [ForeignKey("TrainerId")]
        public virtual Trainer? Trainer { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }
    }

    /// <summary>
    /// Appointment status enumeration
    /// حالة الموعد
    /// </summary>
    public enum AppointmentStatus
    {
        Pending = 0,
        Confirmed = 1,
        Cancelled = 2
    }
}
