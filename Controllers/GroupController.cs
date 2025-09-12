using Microsoft.AspNetCore.Mvc;
using jenkinsCICD.Models;
using jenkinsCICD.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace jenkinsCICD.Controllers
{
    public class GroupController : Controller
    {
        private readonly MockDataService _mockDataService;

        public GroupController(MockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        // GET: Group
        public IActionResult Index()
        {
            var groups = _mockDataService.GetAllActiveGroups();
            return View(groups);
        }

        // GET: Group/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = _mockDataService.GetGroupById(id.Value);
            if (group == null)
            {
                return NotFound();
            }

            // Calculate group statistics
            var totalExpenses = group.Expenses?.Sum(e => e.Amount) ?? 0;
            var totalMembers = group.Members?.Count(m => m.IsActive) ?? 0;
            
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.TotalMembers = totalMembers;
            ViewBag.AveragePerMember = totalMembers > 0 ? totalExpenses / totalMembers : 0;

            return View(group);
        }

        // GET: Group/Create
        public IActionResult Create()
        {
            ViewData["CreatedByUserId"] = new SelectList(_mockDataService.GetAllUsers(), "Id", "Name");
            return View();
        }

        // POST: Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Description,CreatedByUserId")] Group group)
        {
            if (ModelState.IsValid)
            {
                _mockDataService.CreateGroup(group);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CreatedByUserId"] = new SelectList(_mockDataService.GetAllUsers(), "Id", "Name", group.CreatedByUserId);
            return View(group);
        }

        // GET: Group/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = _mockDataService.GetGroupById(id.Value);
            if (group == null)
            {
                return NotFound();
            }

            ViewData["CreatedByUserId"] = new SelectList(_mockDataService.GetAllUsers(), "Id", "Name", group.CreatedByUserId);
            return View(group);
        }

        // POST: Group/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Description,CreatedByUserId,CreatedAt,IsActive")] Group group)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updated = _mockDataService.UpdateGroup(group);
                if (!updated)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CreatedByUserId"] = new SelectList(_mockDataService.GetAllUsers(), "Id", "Name", group.CreatedByUserId);
            return View(group);
        }

        // GET: Group/AddMember/5
        public IActionResult AddMember(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = _mockDataService.GetGroupById(id.Value);
            if (group == null)
            {
                return NotFound();
            }

            var availableUsers = _mockDataService.GetUsersNotInGroup(id.Value);

            ViewBag.GroupId = id;
            ViewBag.GroupName = group.Name;
            ViewData["UserId"] = new SelectList(availableUsers, "Id", "Name");

            return View();
        }

        // POST: Group/AddMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMember(int groupId, int userId, bool isAdmin = false)
        {
            // This would be implemented in MockDataService if needed
            // For demo purposes, just redirect back
            return RedirectToAction(nameof(Details), new { id = groupId });
        }
    }
}
