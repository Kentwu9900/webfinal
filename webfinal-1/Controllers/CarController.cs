using Microsoft.AspNetCore.Mvc;
using webfinal_1.Models;
using System;
using System.Collections.Generic;

namespace webfinal_1.Controllers
{
    public class CarController : Controller
    {
        // 汽車卡池
        private static readonly List<Card> CardPool = new List<Card>
        {
            new Card { Name = "bentley flying spur", ImageUrl = "/car card/bentley flying spur.jpg" },
            new Card { Name = "bentley mulsanne", ImageUrl = "/car card/bentley mulsanne.jpg" },
            new Card { Name = "benz e300", ImageUrl = "/car card/benz e300.jpg" },
            new Card { Name = "benz usa", ImageUrl = "/car card/benz usa.jpg" },
            new Card { Name = "bmw x3", ImageUrl = "/car card/bmw x3.jpg" },
            new Card { Name = "bmw x4", ImageUrl = "/car card/bmw x4.jpg" },
            new Card { Name = "porsche 911 gt3 rs", ImageUrl = "/car card/porsche 911 gt3 rs.jpg" },
            new Card { Name = "porsche 911", ImageUrl = "/car card/porsche 911.jpg" },
            new Card { Name = "toyota gr", ImageUrl = "/car card/toyota gr.jpg" },
            new Card { Name = "toyota supra", ImageUrl = "/car card/toyota supra.jpg" }
        };

        private static readonly Random rng = new Random();

        // 隨機抽一張
        private Card DrawCard()
        {
            return CardPool[rng.Next(CardPool.Count)];
        }

        // GET: /Car
        public IActionResult Index()
        {
            return View();  // 對應到 Views/Car/Index.cshtml
        }

        // POST: /Car/SingleDraw
        [HttpPost]
        public IActionResult SingleDraw()
        {
            var result = new List<Card> { DrawCard() };
            return View("Result", result);
        }

        // POST: /Car/TenDraw
        [HttpPost]
        public IActionResult TenDraw()
        {
            var result = new List<Card>();
            for (int i = 0; i < 10; i++)
                result.Add(DrawCard());
            return View("Result", result);
        }
    }
}
