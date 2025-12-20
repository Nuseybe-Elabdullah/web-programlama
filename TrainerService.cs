using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementSystem.Models
{
    /// <summary>
    /// Many-to-many relationship between Trainer and Service
    /// علاقة متعدد لمتعدد بين المدرب والخدمة
    /// </summary>
    public class TrainerService
    {
        public int TrainerId { get; set; }
        public int ServiceId { get; set; }

        // Navigation properties
        [ForeignKey("TrainerId")]
        public virtual Trainer? Trainer { get; set; }
        
        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }
    }
}
