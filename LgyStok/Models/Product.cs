using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LgyStok.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad zorunlu")]
        public string Name { get; set; }

        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Sku { get; set; }

        [Range(0.01, 9999999999999999.99, ErrorMessage = "Fiyat 0'dan büyük olmalı")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stok 0 veya daha büyük olmalı")]
        public int Stock { get; set; }
    }
}