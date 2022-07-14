using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemPrice { get; set; }
        public int IngredienteId { get; set; }

        public virtual Ingrediente Ingrediente { get; set; }
    }
}