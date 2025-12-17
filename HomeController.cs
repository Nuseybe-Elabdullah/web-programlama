using _1projedeneme.Data;
using _1projedeneme.Models;
using Microsoft.AspNetCore.Mvc;

namespace _1projedeneme.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hizmetler = _context.Hizmetler.ToList();
            return View(hizmetler);
        }
    }
}
