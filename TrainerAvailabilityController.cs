using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymManagementSystem.Data;
using GymManagementSystem.Models;

namespace GymManagementSystem.Controllers
{
    /// <summary>
    /// Controller for trainer availability CRUD operations (Admin only)
    /// متحكم عمليات إدارة توفر المدربين (للمسؤول فقط)
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class TrainerAvailabilityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainerAvailabilityController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrainerAvailability
        public async Task<IActionResult> Index()
        {
            var availabilities = await _context.TrainerAvailabilities
                .Include(t => t.Trainer)
                .OrderBy(t => t.TrainerId)
                .ThenBy(t => t.DayOfWeek)
                .ToListAsync();
            return View(availabilities);
        }

        // GET: TrainerAvailability/Create
        public IActionResult Create()
        {
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName");
            return View();
        }

        // POST: TrainerAvailability/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TrainerId,DayOfWeek,FromTime,ToTime")] TrainerAvailability trainerAvailability)
        {
            if (ModelState.IsValid)
            {
                if (trainerAvailability.FromTime >= trainerAvailability.ToTime)
                {
                    ModelState.AddModelError("ToTime", "End time must be after start time.");
                    ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", trainerAvailability.TrainerId);
                    return View(trainerAvailability);
                }

                _context.Add(trainerAvailability);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Availability created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", trainerAvailability.TrainerId);
            return View(trainerAvailability);
        }

        // GET: TrainerAvailability/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerAvailability = await _context.TrainerAvailabilities.FindAsync(id);
            if (trainerAvailability == null)
            {
                return NotFound();
            }
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", trainerAvailability.TrainerId);
            return View(trainerAvailability);
        }

        // POST: TrainerAvailability/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TrainerId,DayOfWeek,FromTime,ToTime")] TrainerAvailability trainerAvailability)
        {
            if (id != trainerAvailability.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (trainerAvailability.FromTime >= trainerAvailability.ToTime)
                {
                    ModelState.AddModelError("ToTime", "End time must be after start time.");
                    ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", trainerAvailability.TrainerId);
                    return View(trainerAvailability);
                }

                try
                {
                    _context.Update(trainerAvailability);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Availability updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerAvailabilityExists(trainerAvailability.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", trainerAvailability.TrainerId);
            return View(trainerAvailability);
        }

        // GET: TrainerAvailability/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerAvailability = await _context.TrainerAvailabilities
                .Include(t => t.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainerAvailability == null)
            {
                return NotFound();
            }

            return View(trainerAvailability);
        }

        // POST: TrainerAvailability/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainerAvailability = await _context.TrainerAvailabilities.FindAsync(id);
            if (trainerAvailability != null)
            {
                _context.TrainerAvailabilities.Remove(trainerAvailability);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Availability deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TrainerAvailabilityExists(int id)
        {
            return _context.TrainerAvailabilities.Any(e => e.Id == id);
        }
    }
}
