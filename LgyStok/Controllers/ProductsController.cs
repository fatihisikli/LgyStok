using LgyStok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        //formu boş olarak basıyoruz create olduğu için.
        public ActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            // sku kontrol ediyor, aynı var mı diye
            bool skuVar = db.Products.Any(p => p.Sku == product.Sku);
            if (skuVar)
            {
                ModelState.AddModelError("Sku", "Bu Sku zaten kullanılıyor");   // Sku alanının yanında çıkar
            }

            // Kural ihlali varsa formu aynı bilgilerle geri göster
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            db.Products.Add(product);
            db.SaveChanges();   
            return RedirectToAction("Index");   
        }


        //Editlenen formu doluu gösteriyorum
        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);   // Id'ye göre ürünü bul
            if (product == null)
            {
                return HttpNotFound();   // ürün yoksa 404
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            // Aynı Sku başka üründe var mı? (kendisi hariç)
            bool skuVar = db.Products.Any(p => p.Sku == product.Sku && p.Id != product.Id);
            if (skuVar)
            {
                ModelState.AddModelError("Sku", "Bu Sku zaten kullanılıyor");
            }

            // Kural ihlali varsa formu aynı bilgilerle geri göster
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            // DB'deki ürünü bul, alanları güncelle
            var mevcut = db.Products.Find(product.Id);
            mevcut.Name = product.Name;
            mevcut.Sku = product.Sku;
            mevcut.Price = product.Price;
            mevcut.Stock = product.Stock;
            db.SaveChanges(); 

            return RedirectToAction("Index"); 
        }
    }
}