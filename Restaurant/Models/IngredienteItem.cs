using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class IngredienteItem
    {
        public int ItemId { get; set; }
        public int IngredienteId { get; set; }

        public virtual Ingrediente Ingrediente { get; set; }
        public virtual Item Item { get; set; }
    }
}