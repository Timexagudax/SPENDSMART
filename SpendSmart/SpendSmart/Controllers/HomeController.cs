using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SpendSmartDbContext _context;

        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Expenses()
        {

            var allExpenses = _context.Expenses.Include(e => e.User).ToList();

            var totalExpenses = allExpenses.Sum(x => x.Value);

            ViewBag.Expenses = totalExpenses; 
            return View(allExpenses);
        }
        public IActionResult CreateEditExpense(int? id)
        {
            ViewBag.Users = new SelectList(_context.Users, "UserId", "Name");

            if (id != null)
            {

                //editing -> load an expense by id
                var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
                return View(expenseInDb);
            }
            
            return View();
        }

        public IActionResult DeleteExpense(int id)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            _context.Expenses.Remove(expenseInDb);
            _context.SaveChanges();
            return RedirectToAction("Expenses");
        }

        [HttpPost]
        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if (model.Id == 0)
            {
                // Generate automatic serial number
                model.SerialNumber = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();

                _context.Expenses.Add(model);
            }
            else
            {
                // Don’t overwrite serial number when editing
                var existingExpense = _context.Expenses.Find(model.Id);
                if (existingExpense != null)
                {
                    existingExpense.Value = model.Value;
                    existingExpense.Description = model.Description;
                    existingExpense.Color = model.Color;
                    existingExpense.Quantity= model.Quantity;
                    existingExpense.Size = model.Size;
                    // Keep existingExpense.SerialNumber
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Expenses");
        }

        public IActionResult Details(int id)
        {
            var expense = _context.Expenses.FirstOrDefault(e => e.Id == id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
