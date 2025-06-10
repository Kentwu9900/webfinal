using Microsoft.AspNetCore.Mvc;
using webfinal_1.Models;
using System.Collections.Generic;
using System.Linq;

namespace webfinal_1.Controllers
{
    public class PetController : Controller
    {
        private static readonly Dictionary<string, List<Card>> AllCardPools = new Dictionary<string, List<Card>>
        {
            ["animal"] = new List<Card>
            {
                new Card { Name = "熊貓", ImageUrl = "/pet card/1.jpg" },
                new Card { Name = "馬", ImageUrl = "/pet card/2.jpg" },
                new Card { Name = "狐狸", ImageUrl = "/pet card/3.jpg" },
                new Card { Name = "小狗", ImageUrl = "/pet card/4.jpg" },
                new Card { Name = "黑狗", ImageUrl = "/pet card/5.jpg" },
                new Card { Name = "海豹", ImageUrl = "/pet card/6.jpg" },
                new Card { Name = "烏龜", ImageUrl = "/pet card/7.jpg" },
                new Card { Name = "鳥", ImageUrl = "/pet card/8.jpg" },
                new Card { Name = "貓頭鷹", ImageUrl = "/pet card/9.jpg" },
                new Card { Name = "貓咪", ImageUrl = "/pet card/10.jpg" }
            },
            ["car"] = new List<Card>
            {
                new Card { Name = "Bentley Flying Spur", ImageUrl = "/car card/bentley flying spur.jpg" },
                new Card { Name = "Bentley Mulsanne", ImageUrl = "/car card/bentley mulsanne.jpg" },
                new Card { Name = "Benz E300", ImageUrl = "/car card/benz e300.jpg" },
                new Card { Name = "Benz USA", ImageUrl = "/car card/benz usa.jpg" },
                new Card { Name = "BMW X3", ImageUrl = "/car card/bmw x3.jpg" },
                new Card { Name = "BMW X4", ImageUrl = "/car card/bmw x4.jpg" },
                new Card { Name = "Porsche 911 GT3 RS", ImageUrl = "/car card/porsche 911 gt3 rs.jpg" },
                new Card { Name = "Porsche 911", ImageUrl = "/car card/porsche 911.jpg" },
                new Card { Name = "Toyota GR", ImageUrl = "/car card/toyota gr.jpg" },
                new Card { Name = "Toyota Supra", ImageUrl = "/car card/toyota supra.jpg" }
            },
            ["meme"] = new List<Card>
            {
                new Card { Name = "NICE", ImageUrl = "/meme/NICE.jpg" },
                new Card { Name = "ok", ImageUrl = "/meme/ok.jpg" },
                new Card { Name = "WTF", ImageUrl = "/meme/WTF.jpg" },
                new Card { Name = "好喔", ImageUrl = "/meme/好喔.jpg" },
                new Card { Name = "我就爛", ImageUrl = "/meme/我就爛.jpg" },
                new Card { Name = "星爆中", ImageUrl = "/meme/星爆中.jpg" },
                new Card { Name = "哭pepe", ImageUrl = "/meme/哭pepe.jpg" },
                new Card { Name = "黑人問號", ImageUrl = "/meme/黑人問號.jpg" },
                new Card { Name = "亂", ImageUrl = "/meme/亂.jpg" },
                new Card { Name = "謝謝", ImageUrl = "/meme/謝謝.jpg" }
            }
        };

        private static readonly Dictionary<string, CardStats> cardStatistics = new Dictionary<string, CardStats>();
        private static readonly Random rng = new Random();

        private List<Card> GetCardPool(string type) =>
            AllCardPools.ContainsKey(type) ? AllCardPools[type] : AllCardPools["animal"];

        public IActionResult Index(string type = "animal")
        {
            ViewBag.Type = type;
            ViewBag.Message = TempData["Message"];
            return View();
        }

        [HttpPost]
        public IActionResult SingleDraw(string type)
        {
            var pool = GetCardPool(type);
            int? currentPoints = HttpContext.Session.GetInt32("Points");
            if (currentPoints == null || currentPoints < 5)
            {
                TempData["Message"] = "點數不足（需要5點）";
                return RedirectToAction("Index", new { type });
            }

            HttpContext.Session.SetInt32("Points", currentPoints.Value - 5);
            var card = pool[rng.Next(pool.Count)];
            var result = new List<Card> { card };

            var logs = HttpContext.Session.GetObject<List<DrawRecord>>("DrawLogs") ?? new List<DrawRecord>();
            logs.Add(new DrawRecord
            {
                CardName = card.Name,
                ImageUrl = card.ImageUrl,
                Time = DateTime.Now
            });
            HttpContext.Session.SetObject("DrawLogs", logs);

            if (cardStatistics.ContainsKey(card.Name))
                cardStatistics[card.Name].Count++;
            else
                cardStatistics[card.Name] = new CardStats
                {
                    CardName = card.Name,
                    ImageUrl = card.ImageUrl,
                    Count = 1
                };

            return View("Result", result);
        }

        [HttpPost]
        public IActionResult TenDraw(string type)
        {
            var pool = GetCardPool(type);
            int? currentPoints = HttpContext.Session.GetInt32("Points");
            if (currentPoints == null || currentPoints < 50)
            {
                TempData["Message"] = "點數不足（需要50點）";
                return RedirectToAction("Index", new { type });
            }

            HttpContext.Session.SetInt32("Points", currentPoints.Value - 50);

            var result = new List<Card>();
            var logs = HttpContext.Session.GetObject<List<DrawRecord>>("DrawLogs") ?? new List<DrawRecord>();

            for (int i = 0; i < 10; i++)
            {
                var card = pool[rng.Next(pool.Count)];
                result.Add(card);

                logs.Add(new DrawRecord
                {
                    CardName = card.Name,
                    ImageUrl = card.ImageUrl,
                    Time = DateTime.Now
                });

                if (cardStatistics.ContainsKey(card.Name))
                    cardStatistics[card.Name].Count++;
                else
                    cardStatistics[card.Name] = new CardStats
                    {
                        CardName = card.Name,
                        ImageUrl = card.ImageUrl,
                        Count = 1
                    };
            }

            HttpContext.Session.SetObject("DrawLogs", logs);

            return View("Result", result);
        }

        public IActionResult History()
        {
            var logs = HttpContext.Session.GetObject<List<DrawRecord>>("DrawLogs") ?? new List<DrawRecord>();
            return View(logs);
        }

        public IActionResult Ranking()
        {
            var topCards = cardStatistics.Values
                .OrderByDescending(c => c.Count)
                .Take(10)
                .ToList();

            return View(topCards);
        }
    }
}
