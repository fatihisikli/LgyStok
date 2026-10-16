namespace LgyStok.Migrations
{
    using LgyStok.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LgyStok.Models.StokContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LgyStok.Models.StokContext context)
        {
            // Update-Database her çalışınca stokları sıfırlamasın (A senaryosu 98 ve 45 bekliyor)
            if (context.Products.Any()) return;

            context.Products.AddRange(new[]
            {
                new Product { Name = "Kalem",  Sku = "KLM-001", Price = 12.50m,  Stock = 100 },
                new Product { Name = "Defter", Sku = "DFT-001", Price = 45.00m,  Stock = 50 },
                new Product { Name = "Silgi",  Sku = "SLG-001", Price = 8.00m,   Stock = 3 },
                new Product { Name = "Cetvel", Sku = "CTV-001", Price = 15.00m,  Stock = 1 },
                new Product { Name = "Çanta",  Sku = "CNT-001", Price = 350.00m, Stock = 0 },
            });
        }
    }
}
