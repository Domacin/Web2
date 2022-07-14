using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Restaurant.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            Dictionary<string, User> users = (Dictionary<string, User>)HttpContext.Application["users"];
              

            foreach (var item in users.Values)
            {
                if (item.Image == null)
                    item.Image = "a.jpg";
            }
            
            ViewBag.users = users.Values;
            return View("Admin");
        }

        public ActionResult AllOrders()
        {
            List<Ordering> orders = DataBase.LoadAllOrdersFromDataBase();
            Dictionary<string, User> users = DataBase.LoadAllUsers();

            ViewBag.users = users.Values;
            ViewBag.orders = orders;

            return View();
        }

        [HttpPost]
        public ActionResult AddItem(string ItemName,string[] Ingrediente)
        {
            Item i = new Item();
            i.ItemName = ItemName;
            i.ItemPrice = 0; // to start with, we will update it later
            DataBase.AddItemToDataBase(i);

            List<Item> items = DataBase.LoadAllItems();
            List<Ingrediente> ingredientes = DataBase.LoadAllIngredientes();

            foreach (var item in Ingrediente)
            {
                    foreach (var i1 in ingredientes)
                    {
                        if(i1.IngredienteName == item)
                        {
                            Item ii = new Item();
                            ii = items.Last();
                            DataBase.AddItemIngredianteToDataBase(ii.ItemId,i1.IngredienteId);
                            break;
                        }
                    }   
            }

            List<IngredienteItem> list = DataBase.LoadAllIngredienteItemsFromDataBase();
            Item i2 = new Item();
            i2 = items.Last();
            int totalPrice = 0
                ;
            foreach (var item in list)
            {
                if(item.ItemId == i2.ItemId)
                {
                    foreach (var item2 in ingredientes)
                    {
                        if(item2.IngredienteId == item.IngredienteId)
                        {
                            totalPrice += item2.IngredientePrice;
                        }
                    }
                }
               
            }
            //now we update the price in item table
            DataBase.UpdateItemPrice(totalPrice,i2.ItemId);

            ViewBag.message = "Item added to data base";
            ViewBag.ingredientes = ingredientes;
            return View("ItemPanel");
        }
        public ActionResult ItemPanel()
        {
            List<Ingrediente> ingredientes = DataBase.LoadAllIngredientes();
            ViewBag.ingredientes = ingredientes;
            return View("ItemPanel");
        }
        public ActionResult IngredientePanel()
        {
            return View("IngredientePanel");
        }

        [HttpPost]
        public ActionResult AddIngrediente(Ingrediente i)
        {
            DataBase.AddIngredienteToDataBase(i);
            ViewBag.Message = "Ingrediente added to database";
            return View("IngredientePanel");
        }

        public ActionResult Activate(string username)
        {
            Dictionary<string, User> users = DataBase.LoadAllUsers();

            for (int i = 0; i < users.Count; i++)
            {
                var item = users.ElementAt(i);
                var key = item.Key;

                if (item.Value.UserName == username)
                {
                    item.Value.Active = true;
                    DataBase.ActivateUser(item.Value.UserId);
                    break;
                }

            }
            ViewBag.users = users.Values;
            return View("Admin");
        }
        public ActionResult Verify(string username)
        {
            Dictionary<string, User> users = DataBase.LoadAllUsers();

            for (int i = 0; i < users.Count; i++)
            {
                var item = users.ElementAt(i);
                var key = item.Key;

                if (item.Value.UserName == username)
                {
                    item.Value.Verified = true;
                    DataBase.VerifyUser(item.Value.UserId);
                    break;
                }
                   
            }
            ViewBag.users = users.Values;
            return View("Admin");
        }

    }
}