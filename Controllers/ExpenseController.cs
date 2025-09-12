using Microsoft.AspNetCore.Mvc;
using jenkinsCICD.Models;
using jenkinsCICD.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace jenkinsCICD.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly MockDataService _mockDataService;

        public ExpenseController(MockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        // GET: Expense
        public IActionResult Index()
        {
            var expenses = _mockDataService.GetAllExpenses();
            return View(expenses);
        }

        // GET: Expense/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = _mockDataService.GetExpenseById(id.Value);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expense/Create
        public IActionResult Create()
        {
            PopulateDropdownLists();
            return View();
        }

        // POST: Expense/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Description,Amount,ExpenseDate,Category,PaidByUserId,GroupId")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _mockDataService.CreateExpense(expense);
                return RedirectToAction(nameof(Index));
            }
            
            PopulateDropdownLists();
            return View(expense);
        }

        // GET: Expense/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = _mockDataService.GetExpenseById(id.Value);
            if (expense == null)
            {
                return NotFound();
            }

            PopulateDropdownLists();
            return View(expense);
        }

        // POST: Expense/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Description,Amount,ExpenseDate,Category,PaidByUserId,GroupId,CreatedAt")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updated = _mockDataService.UpdateExpense(expense);
                if (!updated)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            
            PopulateDropdownLists();
            return View(expense);
        }

        // GET: Expense/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = _mockDataService.GetExpenseById(id.Value);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _mockDataService.DeleteExpense(id);
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdownLists()
        {
            ViewData["PaidByUserId"] = new SelectList(_mockDataService.GetAllUsers(), "Id", "Name");
            ViewData["GroupId"] = new SelectList(_mockDataService.GetAllActiveGroups(), "Id", "Name");
            ViewData["Categories"] = new SelectList(new[]
            {
                "Ăn uống", "Đi lại", "Mua sắm", "Giải trí", "Y tế", "Giáo dục", "Khác"
            });
        }
    }
}
