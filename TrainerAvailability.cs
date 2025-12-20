using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    /// <summary>
    /// Represents trainer availability schedule
    /// يمثل جدول توفر المدرب
    /// </summary>
    public class TrainerAvailability
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Antrenör")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Haftanın günü gereklidir")]
        [Range(0, 6, ErrorMessage = "Gün 0 (Pazar) ile 6 (Cumartesi) arasında olmalıdır")]
        [Display(Name = "Haftanın Günü")]
        public int DayOfWeek { get; set; }

        [Required(ErrorMessage = "Başlangıç saati gereklidir")]
        [Display(Name = "Başlangıç Saati")]
        public TimeSpan FromTime { get; set; }

        [Required(ErrorMessage = "Bitiş saati gereklidir")]
        [Display(Name = "Bitiş Saati")]
        public TimeSpan ToTime { get; set; }

        // Navigation property
        [ForeignKey("TrainerId")]
        public virtual Trainer? Trainer { get; set; }

        /// <summary>
        /// Gets the day name in English
        /// </summary>
        [NotMapped]
        public string DayName => ((DayOfWeek)DayOfWeek).ToString();
    }
}
