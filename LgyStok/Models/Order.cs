using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LgyStok.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }

        public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();
    }
}