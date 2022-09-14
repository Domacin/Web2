using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Restaurant.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View("Register");
        }


        [HttpPost]
        public ActionResult AddUser(User u,string RepeatedPassword)
        {
            Dictionary<string, User> users = (Dictionary<string, User>)HttpContext.Application["users"];

            if (users.ContainsKey(u.UserName))
            {
                ViewBag.ErrorMessage = "User name is taken";
                return View("Register");
            }

            if(u.Password != RepeatedPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match";
                return View("Register");
            }

            if (u.Tip == Models.User.TIP.CONSUMER)
            {
                u.Verified = true;
                u.Active = true;
            }
            else
            {
                u.Verified = false;
                u.Verified = false;
            }
                

            
            DataBase.AddUserToDataBase(u);
            string pass = DataBase.sha256(u.Password);
            u.Password = pass;
            users.Add(u.UserName, u);
            Session["users"] = users;
            
            return RedirectToAction("Index", "Home");
        }
    }
}