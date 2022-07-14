using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class Ordering
    {
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public int OrderPrice { get; set; }
        public int ClientId { get; set; }      
        public bool Accepted { get; set; }
        public bool Completed { get; set; }

        public virtual User User { get; set; }
        public virtual Item Item { get; set; }
    }
}