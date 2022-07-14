using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Restaurant.Controllers
{
    public class CustomerController : Controller
    {
        
        public ActionResult MyOrders()
        {
            List<Ordering> orders = DataBase.LoadAllOrdersFromDataBase();
            Dictionary<string,User> users = DataBase.LoadAllUsers();
           

            string username = (string)Session["user"];

            User u = new User();
            u = users[username];

            List<Ordering> userOrders = new List<Ordering>();
            

            foreach (var item in orders)
            {
                if(item.ClientId == u.UserId)
                {
                    userOrders.Add(item);
                }
            }

        

            ViewBag.orders = userOrders;//here we have all orders of particular user
            

            return View();
        }

        public ActionResult ConfirmOrder()
        {
            List<Item> items = DataBase.LoadAllItems();
            
            string firstDish = (string)TempData["name1"];
            string secondDish = (string)TempData["name2"];
            string thirdDish = (string)TempData["name3"];

            int price1 = (int)TempData["price1"];
            int price2 = (int)TempData["price2"];
            int price3 = (int)TempData["price3"];

            ViewBag.dish1 = "";
            ViewBag.dish2 = "";
            ViewBag.dish3 = "";

            ViewBag.price = 0;
            int price = price1 + price2 + price3 + 15;//is for service of delivery

            foreach (var item in items)
            {
                if (item.ItemName == firstDish)
                {
                    ViewBag.dish1 = firstDish;                 
                }
                    

                if (item.ItemName == secondDish)
                {
                    ViewBag.dish2 = secondDish;
                }
                    

                if (item.ItemName == thirdDish)
                {
                    ViewBag.dish3 = thirdDish;
                }
                   
            }

            ViewBag.price = price;
            return View("ConfirmOrder");
        }


        [HttpPost]
        public ActionResult FinaliseOrder(int totalPrice, string firstDish="", string secondDish="", string thirdDish="")
        {
            Dictionary<string, User> users = DataBase.LoadAllUsers();
            List<Item> items = DataBase.LoadAllItems();
            List<Ordering> orders = new List<Ordering>();
            string username = (string)Session["user"];
            Ordering o = new Ordering();
            o.Amount = (int)TempData["totalAmount"];
            o.Comment = (string)TempData["Comment"];
            o.Address = (string)TempData["Address"];
            o.OrderPrice = totalPrice;
            o.Accepted = false;
            o.Completed = false;
            o.ClientId = users[username].UserId;
            DataBase.AddOrderToDataBase(o);

            orders = DataBase.LoadAllOrdersFromDataBase();

            o.OrderId = orders.Last().OrderId;

            if (!string.IsNullOrEmpty(firstDish))
            {
                foreach (var item in items)
                {
                    if(item.ItemName == firstDish)
                    {
                        Item i = new Item();
                        i = item;
                        DataBase.AddItemToOrder(i.ItemId, o.OrderId);
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(secondDish))
            {
                foreach (var item in items)
                {
                    if (item.ItemName == secondDish)
                    {
                        Item i = new Item();
                        i = item;
                        DataBase.AddItemToOrder(i.ItemId, o.OrderId);
                        break;
                    }
                }
            }


            if (!string.IsNullOrEmpty(thirdDish))
            {
                foreach (var item in items)
                {
                    if (item.ItemName == thirdDish)
                    {
                        Item i = new Item();
                        i = item;
                        DataBase.AddItemToOrder(i.ItemId, o.OrderId);
                        break;
                    }
                }
            }

            return RedirectToAction("MyOrders");
        }
        // GET: Customer
        public ActionResult Index()
        {
            List<Item> items = DataBase.LoadAllItems();
            
            ViewBag.items = items;
            return View("OrderFood");
        }

        [HttpPost]
        public ActionResult Order(string Dish1,string Dish2,string Dish3,string Address,string Comment = "", int amount1 = 0, int amount2 = 0 ,int amount3 = 0)
        {
            List<Item> items = DataBase.LoadAllItems();
            //setting defaults values in case some of the dishes weren't selected
            TempData["price1"] = 0;
            TempData["name1"] = "";

            TempData["price2"] = 0;
            TempData["name2"] = "";

            TempData["price3"] = 0;
            TempData["name3"] = "";

            TempData["amount1"] = amount1;
            TempData["amount2"] = amount2;
            TempData["amount3"] = amount3;

            TempData["comment"] = Comment;
            int totalAmount = amount1 + amount2 + amount3;
            TempData["totalAmount"] = totalAmount;

            TempData["Address"] = Address;
            TempData["Comment"] = Comment;
            //if we selected first dish
            if (!string.IsNullOrEmpty(Dish1))
            {
                foreach (var item in items)
                {
                    if(item.ItemName == Dish1)
                    {
                        int price = 0;
                        if (amount1 > 0)
                        {
                             price += item.ItemPrice * amount1;
                        }
                        else
                        {
                             price += item.ItemPrice;
                        }
                       
                        
                        string name = item.ItemName;
                        TempData["price1"] = price;
                        TempData["name1"] = name;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(Dish2))
            {
                foreach (var item in items)
                {
                    if (item.ItemName == Dish2)
                    {
                        int price = 0;
                        if (amount2 > 0)
                        {
                            price += item.ItemPrice * amount2;
                        }
                        else
                        {
                            price += item.ItemPrice;
                        }
                        string name = item.ItemName;
                        TempData["price2"] = price;
                        TempData["name2"] = name;
                        break;
                    }
                }
            }


            if (!string.IsNullOrEmpty(Dish3))
            {
                foreach (var item in items)
                {
                    if (item.ItemName == Dish3)
                    {
                        int price = 0;
                        if (amount3 > 0)
                        {
                            price += item.ItemPrice * amount3;
                        }
                        else
                        {
                            price += item.ItemPrice;
                        }

                        string name = item.ItemName;
                        TempData["price3"] = price;
                        TempData["name3"] = name;
                        break;
                    }
                }
            }

            return RedirectToAction("ConfirmOrder");
        }
    }
}