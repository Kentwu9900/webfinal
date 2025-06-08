using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using webfinal_1.Models;

namespace webfinal_1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // ✅ 每日登入送點功能
        string lastRewardDate = HttpContext.Session.GetString("LastRewardDate");
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        if (lastRewardDate != today)
        {
            int currentPoints = HttpContext.Session.GetInt32("Points") ?? 0;
            HttpContext.Session.SetInt32("Points", currentPoints + 5);
            HttpContext.Session.SetString("LastRewardDate", today);
            TempData["RewardMessage"] = "🎁 今日登入獎勵 +5 點！";
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult SelectZone()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
