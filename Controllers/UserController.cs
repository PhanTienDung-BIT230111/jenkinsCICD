using Microsoft.AspNetCore.Mvc;
using jenkinsCICD.Models;
using jenkinsCICD.Services;

namespace jenkinsCICD.Controllers
{
    public class UserController : Controller
    {
        private readonly MockDataService _mockDataService;

        public UserController(MockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        // GET: User
        public IActionResult Index()
        {
            var users = _mockDataService.GetAllUsers();
            return View(users);
        }

        // GET: User/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _mockDataService.GetUserById(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            // Calculate user statistics
            var totalExpenses = user.Expenses?.Sum(e => e.Amount) ?? 0;
            var activeGroups = user.GroupMemberships?.Count(gm => gm.IsActive) ?? 0;
            
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.ActiveGroups = activeGroups;

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Email,PhoneNumber")] User user)
        {
            if (ModelState.IsValid)
            {
                _mockDataService.CreateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _mockDataService.GetUserById(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Email,PhoneNumber,CreatedAt")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updated = _mockDataService.UpdateUser(user);
                if (!updated)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _mockDataService.GetUserById(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _mockDataService.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
