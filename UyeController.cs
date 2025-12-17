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
    public class UyeController : Controller
    {
        private readonly AppDbContext _context;

        public UyeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Uye
        public async Task<IActionResult> Index()
        {
            return View(await _context.Uyeler.ToListAsync());
        }

        // GET: Uye/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uye == null)
            {
                return NotFound();
            }

            return View(uye);
        }

        // GET: Uye/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uye/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ad,Email,Telefon")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uye);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uye);
        }

        // GET: Uye/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uye = await _context.Uyeler.FindAsync(id);
            if (uye == null)
            {
                return NotFound();
            }
            return View(uye);
        }

        // POST: Uye/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Email,Telefon")] Uye uye)
        {
            if (id != uye.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uye);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UyeExists(uye.Id))
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
            return View(uye);
        }

        // GET: Uye/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uye = await _context.Uyeler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uye == null)
            {
                return NotFound();
            }

            return View(uye);
        }

        // POST: Uye/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uye = await _context.Uyeler.FindAsync(id);
            if (uye != null)
            {
                _context.Uyeler.Remove(uye);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UyeExists(int id)
        {
            return _context.Uyeler.Any(e => e.Id == id);
        }
    }
}
