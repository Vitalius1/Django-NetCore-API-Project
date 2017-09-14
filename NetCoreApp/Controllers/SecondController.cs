using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Auction.Models;
using System.Linq;
using Newtonsoft.Json;


namespace Auction.Controllers
{
    public class SecondController : Controller
    {
        private AuctionContext _context;

        public SecondController(AuctionContext context)
        {
            _context = context;
        }


        // GET: /Home/
        [HttpGet]
        [Route("dashboard/{id}")]
        public IActionResult Index(int id = 1)
        {
            // if (HttpContext.Session.GetInt32("UserId") == null)
            // {
            //     TempData["NiceTry"] = "You need to be logged in to view account info!";
            //     return RedirectToAction("LoginPage", "Register");
            // }
            var Products = new List<Dictionary<string, object>>();
            WebRequest.GetProductDataAsync(id, ApiResponse =>
                {
                    Products = ApiResponse;
                }
            ).Wait();
            ViewBag.Products = Products;
            return View("Dashboard");
        }
// ============================================================================================================

        [HttpGet]
        [Route("getProduct/{id}")]
        public IActionResult OneProduct(int id)
        {
            // if (HttpContext.Session.GetInt32("UserId") == null)
            // {
            //     TempData["NiceTry"] = "You need to be logged in to view account info!";
            //     return RedirectToAction("LoginPage", "Register");
            // }
            var ProductInfo = new Dictionary<string, object>();
            WebRequest.SingleProductAsync(id, ApiResponse =>
                {
                    ProductInfo = ApiResponse;
                }
            ).Wait();
            System.Console.WriteLine(ProductInfo["product_end_date"]);
            ViewBag.Product = ProductInfo;
            return View("OneProduct");
        }
    }
}