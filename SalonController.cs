using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _1projedeneme.Models;
using _1projedeneme.Data;


namespace _1projedeneme.Controllers
{
    public class SalonController : Controller
    {
        private readonly AppDbContext _context;

        public SalonController(AppDbContext context)
        {
            _context = context;
        }

        // Antrenörleri listeleme
        public async Task<IActionResult> Index()
        {
            var antrenorler = await _context.Antrenorler.Include(a => a.Salon).ToListAsync();
            return View(antrenorler);
        }

        // Antrenör detaylarını görüntüleme
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenor = await _context.Antrenorler.Include(a => a.Salon).FirstOrDefaultAsync(m => m.AntrenorId == id);
            if (antrenor == null)
            {
                return NotFound();
            }

            return View(antrenor);
        }

        // Antrenör oluşturma (Admin paneli)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ad, UzmanlikAlani, Musaitlik, SalonId")] Antrenor antrenor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(antrenor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(antrenor);
        }

        // Antrenör düzenleme (Admin paneli)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenor = await _context.Antrenorler.FindAsync(id);
            if (antrenor == null)
            {
                return NotFound();
            }
            return View(antrenor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AntrenorId, Ad, UzmanlikAlani, Musaitlik, SalonId")] Antrenor antrenor)
        {
            if (id != antrenor.AntrenorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(antrenor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AntrenorExists(antrenor.AntrenorId))
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
            return View(antrenor);
        }

        private bool AntrenorExists(int id)
        {
            return _context.Antrenorler.Any(e => e.AntrenorId == id);

        }
    }
}
