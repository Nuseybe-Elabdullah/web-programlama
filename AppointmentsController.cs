using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymManagementSystem.Data;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using GymManagementSystem.Services;

namespace GymManagementSystem.Controllers
{
    /// <summary>
    /// Controller for appointment booking and management
    /// متحكم حجز وإدارة المواعيد
    /// </summary>
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IAppointmentService appointmentService)
        {
            _context = context;
            _userManager = userManager;
            _appointmentService = appointmentService;
        }

        // GET: Appointments (Admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Member)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .OrderByDescending(a => a.Date)
                .ThenByDescending(a => a.StartTime)
                .ToListAsync();

            return View(appointments);
        }

        // GET: Appointments/MyAppointments
        public async Task<IActionResult> MyAppointments()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            // Get appointments without ordering (SQLite can't order TimeSpan)
            var appointments = await _context.Appointments
                .Include(a => a.Member)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Where(a => a.MemberId == user.Id)
                .ToListAsync();
                
            // Order in memory
            appointments = appointments
                .OrderByDescending(a => a.Date)
                .ThenByDescending(a => a.StartTime)
                .ToList();

            return View(appointments);
        }

        // GET: Appointments/Book
        public async Task<IActionResult> Book()
        {
            var model = new BookAppointmentViewModel
            {
                Trainers = await _context.Trainers
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = $"{t.FullName} - {t.Specialization}"
                    })
                    .ToListAsync()
            };

            return View(model);
        }

        // POST: Appointments/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                
                // Parse selected time
                if (!TimeSpan.TryParse(model.SelectedTime, out TimeSpan startTime))
                {
                    ModelState.AddModelError("SelectedTime", "Invalid time format.");
                    await PopulateBookingModel(model);
                    return View(model);
                }

                // Get service duration
                var service = await _context.Services.FindAsync(model.ServiceId);
                if (service == null)
                {
                    ModelState.AddModelError("", "Service not found.");
                    await PopulateBookingModel(model);
                    return View(model);
                }

                var endTime = startTime.Add(TimeSpan.FromMinutes(service.Duration));

                // Validate appointment
                var isValid = await _appointmentService.ValidateAppointment(
                    model.TrainerId, 
                    model.ServiceId, 
                    model.Date, 
                    startTime);

                if (!isValid)
                {
                    ModelState.AddModelError("", "The selected time slot is not available. Please choose another time.");
                    await PopulateBookingModel(model);
                    return View(model);
                }

                // Create appointment
                var appointment = new Appointment
                {
                    MemberId = user.Id,
                    TrainerId = model.TrainerId,
                    ServiceId = model.ServiceId,
                    Date = model.Date,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = AppointmentStatus.Pending,
                    CreatedAt = DateTime.Now
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Appointment booked successfully! Waiting for confirmation.";
                return RedirectToAction(nameof(MyAppointments));
            }

            await PopulateBookingModel(model);
            return View(model);
        }

        // GET: Appointments/GetServicesByTrainer
        [HttpGet]
        public async Task<IActionResult> GetServicesByTrainer(int trainerId)
        {
            var services = await _context.TrainerServices
                .Where(ts => ts.TrainerId == trainerId)
                .Include(ts => ts.Service)
                .Select(ts => new
                {
                    id = ts.Service.Id,
                    name = ts.Service.Name,
                    price = ts.Service.Price,
                    duration = ts.Service.Duration
                })
                .ToListAsync();

            return Json(services);
        }

        // GET: Appointments/GetAvailableSlots
        [HttpGet]
        public async Task<IActionResult> GetAvailableSlots(int trainerId, int serviceId, DateTime date)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
            {
                return Json(new List<string>());
            }

            var slots = await _appointmentService.GetAvailableTimeSlots(trainerId, date, service.Duration);
            return Json(slots);
        }

        // POST: Appointments/UpdateStatus (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, AppointmentStatus status)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = status;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Appointment status updated to {status}.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Appointments/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Member)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            // Check if user has permission to view this appointment
            var user = await _userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && appointment.MemberId != user?.Id)
            {
                return Forbid();
            }

            return View(appointment);
        }

        // POST: Appointments/Confirm (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = AppointmentStatus.Confirmed;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Randevu onaylandı.";
            return RedirectToAction(nameof(Details), new { id = appointment.Id });
        }

        // POST: Appointments/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            
            // Admin can cancel any appointment, members can only cancel their own
            var appointment = await _context.Appointments.FindAsync(id);
            
            if (appointment == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin") && appointment.MemberId != user?.Id)
            {
                return Forbid();
            }

            appointment.Status = AppointmentStatus.Cancelled;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Randevu iptal edildi.";
            
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Details), new { id = appointment.Id });
            }
            
            return RedirectToAction(nameof(MyAppointments));
        }

        private async Task PopulateBookingModel(BookAppointmentViewModel model)
        {
            model.Trainers = await _context.Trainers
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"{t.FullName} - {t.Specialization}"
                })
                .ToListAsync();

            if (model.TrainerId > 0)
            {
                model.Services = await _context.TrainerServices
                    .Where(ts => ts.TrainerId == model.TrainerId)
                    .Include(ts => ts.Service)
                    .Select(ts => new SelectListItem
                    {
                        Value = ts.Service.Id.ToString(),
                        Text = $"{ts.Service.Name} - ${ts.Service.Price}"
                    })
                    .ToListAsync();
            }
        }
    }
}
