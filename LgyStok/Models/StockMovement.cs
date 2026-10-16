using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LgyStok.Models
{
    public class StockMovement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Change { get; set; }          //  -2
        public string Reason { get; set; }       // "Sipariş #12"
        public DateTime CreatedAt { get; set; }

        public Product Product { get; set; }
    }
}