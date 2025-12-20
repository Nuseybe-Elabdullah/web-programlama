using Microsoft.EntityFrameworkCore;
using GymManagementSystem.Data;
using GymManagementSystem.Models;

namespace GymManagementSystem.Services
{
    /// <summary>
    /// Service for managing appointments with LINQ queries
    /// خدمة إدارة المواعيد باستخدام LINQ
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Check if trainer is available at specified date and time using LINQ
        /// التحقق من توفر المدرب في التاريخ والوقت المحدد
        /// </summary>
        public async Task<bool> IsTrainerAvailable(int trainerId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeAppointmentId = null)
        {
            var dayOfWeek = (int)date.DayOfWeek;

            // Get trainer availability for this day using LINQ
            var availability = await _context.TrainerAvailabilities
                .Where(ta => ta.TrainerId == trainerId && ta.DayOfWeek == dayOfWeek)
                .FirstOrDefaultAsync();

            // Check time range in memory to avoid SQLite TimeSpan limitation
            if (availability == null)
                return false;
                
            // Do time comparison in memory
            if (availability.FromTime > startTime || availability.ToTime < endTime)
                return false;

            // Check for overlapping appointments
            var hasOverlap = await HasOverlappingAppointment(trainerId, date, startTime, endTime, excludeAppointmentId);

            return !hasOverlap;
        }

        /// <summary>
        /// Check if there are overlapping appointments using LINQ
        /// التحقق من وجود مواعيد متداخلة
        /// </summary>
        public async Task<bool> HasOverlappingAppointment(int trainerId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeAppointmentId = null)
        {
            // Get all appointments for trainer on this date
            var query = _context.Appointments
                .Where(a => a.TrainerId == trainerId)
                .Where(a => a.Date.Date == date.Date)
                .Where(a => a.Status != AppointmentStatus.Cancelled);

            if (excludeAppointmentId.HasValue)
            {
                query = query.Where(a => a.Id != excludeAppointmentId.Value);
            }

            // Get data and check overlap in memory
            var appointments = await query.ToListAsync();
            
            Console.WriteLine($"DEBUG: Checking overlap for trainer {trainerId} on {date.Date}");
            Console.WriteLine($"DEBUG: New appointment time: {startTime} - {endTime}");
            Console.WriteLine($"DEBUG: Found {appointments.Count} existing appointments");
            
            foreach (var app in appointments)
            {
                Console.WriteLine($"DEBUG: Existing: {app.StartTime} - {app.EndTime}");
                var overlaps = app.StartTime < endTime && app.EndTime > startTime;
                Console.WriteLine($"DEBUG: Overlaps? {overlaps}");
            }
            
            var result = appointments.Any(a => 
                a.StartTime < endTime && a.EndTime > startTime
            );
            
            Console.WriteLine($"DEBUG: Final overlap result: {result}");
            return result;
        }

        /// <summary>
        /// Get available time slots for a trainer on a specific date using LINQ
        /// الحصول على الأوقات المتاحة للمدرب في تاريخ محدد
        /// </summary>
        public async Task<List<string>> GetAvailableTimeSlots(int trainerId, DateTime date, int serviceDuration)
        {
            var availableSlots = new List<string>();
            var dayOfWeek = (int)date.DayOfWeek;

            // Get trainer availability for the day using LINQ
            var availability = await _context.TrainerAvailabilities
                .Where(ta => ta.TrainerId == trainerId && ta.DayOfWeek == dayOfWeek)
                .FirstOrDefaultAsync();

            if (availability == null)
                return availableSlots;

            // Get all appointments for the trainer on this date using LINQ
            var appointments = await _context.Appointments
                .Where(a => a.TrainerId == trainerId)
                .Where(a => a.Date.Date == date.Date)
                .Where(a => a.Status != AppointmentStatus.Cancelled)
                .Select(a => new { a.StartTime, a.EndTime })
                .ToListAsync();
            
            // Order in memory to avoid SQLite TimeSpan limitation
            appointments = appointments.OrderBy(a => a.StartTime).ToList();

            // Generate time slots
            var currentTime = availability.FromTime;
            var endTime = availability.ToTime;
            var slotDuration = TimeSpan.FromMinutes(serviceDuration);

            while (currentTime.Add(slotDuration) <= endTime)
            {
                var slotEnd = currentTime.Add(slotDuration);

                // Check if this slot overlaps with any appointment using LINQ
                var isSlotAvailable = !appointments.Any(a =>
                    (a.StartTime < slotEnd && a.EndTime > currentTime)
                );

                if (isSlotAvailable)
                {
                    availableSlots.Add(currentTime.ToString(@"hh\:mm"));
                }

                // Move to next slot (30-minute intervals)
                currentTime = currentTime.Add(TimeSpan.FromMinutes(30));
            }

            return availableSlots;
        }

        /// <summary>
        /// Validate appointment booking using LINQ
        /// التحقق من صحة حجز الموعد
        /// </summary>
        public async Task<bool> ValidateAppointment(int trainerId, int serviceId, DateTime date, TimeSpan startTime)
        {
            // Check if service exists and get duration using LINQ
            var service = await _context.Services
                .Where(s => s.Id == serviceId)
                .Select(s => new { s.Duration })
                .FirstOrDefaultAsync();

            if (service == null)
                return false;

            var endTime = startTime.Add(TimeSpan.FromMinutes(service.Duration));

            // Check if date is in the future
            if (date.Date < DateTime.Today)
                return false;

            // Check trainer availability
            return await IsTrainerAvailable(trainerId, date, startTime, endTime);
        }
    }
}
