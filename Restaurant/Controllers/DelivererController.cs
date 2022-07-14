using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Restaurant.Controllers
{
    public class DelivererController : Controller
    {
        // GET: Deliverer
        public ActionResult Index()
        {
            List<Ordering> orders = DataBase.LoadAllOrdersFromDataBase();
            List<Ordering> notAccepted = new List<Ordering>();
            List<Ordering> delivered = new List<Ordering>();//accepted
            List<OrderDeliverer> delivererOrders = DataBase.LoadTakenOrders();
            List<Ordering> showDeliveredForDeliverer = new List<Ordering>();
            Dictionary<string, User> users = DataBase.LoadAllUsers();
            List<Ordering> helpingList = new List<Ordering>();//used to check if delivering has any accepted deliveries
            string username = (string)Session["user"];
            bool verified = users[username].Verified;
            User u = new User();
            u = users[username];
            foreach (var item in orders)
            {
                if(item.Accepted == false )
                {
                    notAccepted.Add(item);
                }else if (item.Accepted == true && item.Completed == false)
                {
                    notAccepted.Add(item);
                    helpingList.Add(item);
                }
                else
                {

                    delivered.Add(item);
                }
            }

            foreach (var item in delivered)
            {
                foreach(var item2 in delivererOrders)
                {
                    if (item.OrderId == item2.Order_ID && item2.Deliverer_ID == u.UserId)
                    {
                        showDeliveredForDeliverer.Add(item);
                    }
                        
                }
              

            }

            ViewBag.count = 0;
            foreach (var item in helpingList)//accepted but not delivered
            {
                foreach (var item2 in delivererOrders)
                {
                   if(item2.Deliverer_ID == u.UserId && item.OrderId == item2.Order_ID)
                    {
                        ViewBag.count = 1; 
                    }

                }


            }

            ViewBag.verified = verified;
            ViewBag.notAccepted = notAccepted;
            ViewBag.delivered = showDeliveredForDeliverer;
            ViewBag.helpingList = helpingList;
            return View("OrdersDeliverer");
        }

        public ActionResult Delivered(int x)
        {
            DataBase.OrderCompleted(x);
            return RedirectToAction("Index");
        }
      
        public ActionResult AcceptOrder(int order)
        {
            Dictionary<string, User> users = DataBase.LoadAllUsers();
            List<Ordering> helpingList = new List<Ordering>();//used to check if delivering has any accepted deliveries
            User u = new User();
            string username = (string)Session["user"];
            u = users[username];

            DataBase.AddOrderToDeliverer(u.UserId, order);
            DataBase.AcceptOrder(order);


            
            return RedirectToAction("Index");
        }
    }
}