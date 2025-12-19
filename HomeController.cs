using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GymManagementSystem.Data;
using GymManagementSystem.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace GymManagementSystem.Controllers
{
    /// <summary>
    /// Home controller for main pages
    /// متحكم الصفحة الرئيسية
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/AdminDashboard
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            ViewBag.TotalGyms = await _context.Gyms.CountAsync();
            ViewBag.TotalTrainers = await _context.Trainers.CountAsync();
            ViewBag.TotalServices = await _context.Services.CountAsync();
            ViewBag.TotalAppointments = await _context.Appointments.CountAsync();
            ViewBag.PendingAppointments = await _context.Appointments
                .Where(a => a.Status == AppointmentStatus.Pending)
                .CountAsync();

            var recentAppointments = await _context.Appointments
                .Include(a => a.Member)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .OrderByDescending(a => a.CreatedAt)
                .Take(10)
                .ToListAsync();

            return View(recentAppointments);
        }

        // GET: /Home/MemberDashboard
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> MemberDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var today = DateTime.Today;
            
            // Get all appointments for this member (no ordering in DB query due to SQLite TimeSpan limitation)
            var allAppointments = await _context.Appointments
                .Include(a => a.Member)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Where(a => a.MemberId == userId)
                .Where(a => a.Status != AppointmentStatus.Cancelled)
                .ToListAsync();
            
            // Filter and sort in memory
            var upcomingAppointments = allAppointments
                .Where(a => a.Date >= today)
                .OrderBy(a => a.Date)
                .ThenBy(a => a.StartTime)
                .Take(5)
                .ToList();

            return View(upcomingAppointments);
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
