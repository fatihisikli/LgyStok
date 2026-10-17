using LgyStok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LgyStok.Controllers
{
    public class OrdersController : Controller
    {
        private StokContext db = new StokContext();

        public ActionResult Create()
        {
            var products = db.Products.OrderBy(p => p.Name).ToList();   // ürünler dropdown için
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]   // token attr
        public ActionResult Create(string customerName, int[] productIds, int[] quantities)
        {
            return Json(new { success = true, message = "Test" });
        }
    }
}