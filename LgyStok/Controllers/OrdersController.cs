using LgyStok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace LgyStok.Controllers
{
    public class OrdersController : Controller
    {
        private StokContext db = new StokContext();

        public ActionResult Index()
        {
            var orders = db.Orders
                .Include(o => o.Lines)
                .OrderByDescending(o => o.Id)
                .ToList();

            return View(orders);
        }

        public ActionResult Create()
        {
            var products = db.Products.OrderBy(p => p.Name).ToList();   // ürünler dropdown için
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]   // token attr
        public ActionResult Create(string customerName, int[] productIds, int[] quantities)
        {
            // En önemli madde 4. madde: sipariş kaydı tek bir SQL Server transaction'ı içinde yapılmalı
            using (var tx = db.Database.BeginTransaction())
            {
                try
                {
                    // Önce sipariş kaydı (Id lazım, "Sipariş #12" yazmak için)
                    var order = new Order();
                    order.CustomerName = customerName;
                    order.CreatedAt = DateTime.Now;
                    db.Orders.Add(order);
                    db.SaveChanges();

                    decimal total = 0;

                    // Her satır için
                    for (int i = 0; i < productIds.Length; i++)
                    {
                        int productId = productIds[i];
                        int quantity = quantities[i];
                        var product = db.Products.Find(productId);

                        // Stok yetmiyorsa her şeyi geri al
                        if (product.Stock < quantity)
                        {
                            tx.Rollback();
                            return Json(new { success = false, message = "Stok yetmiyor: " + product.Name });
                        }

                        // Stoktan düş
                        product.Stock = product.Stock - quantity;

                        // Sipariş satırı 
                        var line = new OrderLine();
                        line.OrderId = order.Id;
                        line.ProductId = productId;
                        line.Quantity = quantity;
                        line.UnitPrice = product.Price;
                        db.OrderLines.Add(line);

                        // Stok hareketi
                        var movement = new StockMovement();
                        movement.ProductId = productId;
                        movement.Change = -quantity;
                        movement.Reason = "Sipariş #" + order.Id;
                        movement.CreatedAt = DateTime.Now;
                        db.StockMovements.Add(movement);

                        total = total + product.Price * quantity;
                    }

                    order.Total = total;
                    db.SaveChanges();
                    tx.Commit();   // commit ile bütün işlemleri kalıcı hale getiriyoruz.

                    return Json(new { success = true, message = "Sipariş oluştu, no: " + order.Id });
                }
                catch (Exception)
                {
                    tx.Rollback();  // bir yerde hata varsa siparişi iptal et.
                    return Json(new { success = false, message = "Sipariş kaydedilemedi" });
                }
            }
        }
    }
}