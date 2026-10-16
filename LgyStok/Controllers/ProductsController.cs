using LgyStok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LgyStok.Controllers
{
    public class ProductsController : Controller
    {
       
        private StokContext db = new StokContext();

        public ActionResult Index(string search, int page = 1)
        {
            int pageSize = 10;

            IQueryable<Product> products = db.Products; // sorgu

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search)); // ada göre filtre
            }

            // sayfa sayısı
            int total = products.Count();
            int totalPages = (int)Math.Ceiling(total / (double)pageSize);
            if (totalPages == 0) totalPages = 1;

            if (page < 1) page = 1; // geçersiz sayfa


            // sadece bu sayfanın kayıtları
            var list = products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // view'a taşıma
            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            return View(list);
        }
    }
}