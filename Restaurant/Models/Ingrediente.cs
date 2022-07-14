using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class Ingrediente
    {
        public int IngredienteId { get; set; }
        public string IngredienteName { get; set; }
        public int IngredientePrice { get; set; }
    }
}