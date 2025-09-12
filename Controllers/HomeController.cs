using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using jenkinsCICD.Models;
using jenkinsCICD.Services;

namespace jenkinsCICD.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MockDataService _mockDataService;

    public HomeController(ILogger<HomeController> logger, MockDataService mockDataService)
    {
        _logger = logger;
        _mockDataService = mockDataService;
    }

    public IActionResult Index()
    {
        // Get overview statistics
        var totalExpenses = _mockDataService.GetTotalExpenses();
        var totalGroups = _mockDataService.GetActiveGroupsCount();
        var recentExpenses = _mockDataService.GetRecentExpenses(5);

        ViewBag.TotalExpenses = totalExpenses;
        ViewBag.TotalGroups = totalGroups;
        ViewBag.RecentExpenses = recentExpenses;

        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
