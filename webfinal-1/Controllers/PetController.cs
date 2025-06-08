using Microsoft.AspNetCore.Mvc;
using webfinal_1.Models;
using System.Collections.Generic;

namespace webfinal_1.Controllers
{
    public class PetController : Controller
    {
        private static readonly List<Card> CardPool = new List<Card>
        {
            new Card { Name = "熊貓", ImageUrl = "/pet card/1.jpg" },
            new Card { Name = "馬", ImageUrl = "/pet card/2.jpg" },
            new Card { Name = "狐狸", ImageUrl = "/pet card/3.jpg" },
            new Card { Name = "青蛙", ImageUrl = "/pet card/4.jpg" },
            new Card { Name = "黑狗", ImageUrl = "/pet card/5.jpg" },
            new Card { Name = "海豹", ImageUrl = "/pet card/6.jpg" },
            new Card { Name = "烏龜", ImageUrl = "/pet card/7.jpg" },
            new Card { Name = "鳥", ImageUrl = "/pet card/8.jpg" },
            new Card { Name = "貓頭鷹", ImageUrl = "/pet card/9.jpg" },
            new Card { Name = "貓咪", ImageUrl = "/pet card/10.jpg" }
        };

        private static readonly Random rng = new Random();

        private Card DrawCard()
        {
            return CardPool[rng.Next(CardPool.Count)];
        }

        public IActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }

        [HttpPost]
        public IActionResult SingleDraw()
        {
            int? currentPoints = HttpContext.Session.GetInt32("Points");
            if (currentPoints == null || currentPoints < 5)
            {
                TempData["Message"] = "點數不足（需要5點）";
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetInt32("Points", currentPoints.Value - 5);

            var result = new List<Card> { DrawCard() };
            return View("Result", result);
        }

        [HttpPost]
        public IActionResult TenDraw()
        {
            int? currentPoints = HttpContext.Session.GetInt32("Points");
            if (currentPoints == null || currentPoints < 50)
            {
                TempData["Message"] = "點數不足（需要50點）";
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetInt32("Points", currentPoints.Value - 50);

            var result = new List<Card>();
            for (int i = 0; i < 10; i++)
                result.Add(DrawCard());

            return View("Result", result);
        }
    }
}
