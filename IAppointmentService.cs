using GymManagementSystem.Models;

namespace GymManagementSystem.Services
{
    /// <summary>
    /// Interface for appointment service
    /// واجهة خدمة المواعيد
    /// </summary>
    public interface IAppointmentService
    {
        Task<bool> IsTrainerAvailable(int trainerId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeAppointmentId = null);
        Task<List<string>> GetAvailableTimeSlots(int trainerId, DateTime date, int serviceDuration);
        Task<bool> HasOverlappingAppointment(int trainerId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeAppointmentId = null);
        Task<bool> ValidateAppointment(int trainerId, int serviceId, DateTime date, TimeSpan startTime);
    }
}
