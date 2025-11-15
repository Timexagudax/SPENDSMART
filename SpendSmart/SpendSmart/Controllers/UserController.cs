using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;

namespace SpendSmart.Controllers
{
    public class UserController : Controller
    {
        private readonly SpendSmartDbContext _dbcontext;

        public UserController(SpendSmartDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dbcontext.Users.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                _dbcontext.Users.Add(user);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
