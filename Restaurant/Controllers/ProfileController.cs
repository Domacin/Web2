using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace Restaurant.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            Dictionary<string, User> users = DataBase.LoadAllUsers();
            string username = (string)Session["user"];
            string active;
            User u = new User();

            u.Active = users[username].Active;
            u.Address = users[username].Address;
            u.DateOfBirth = users[username].DateOfBirth;
            u.FirstName = users[username].FirstName;
            u.LastName = users[username].LastName;
            u.Password = users[username].Password;
            u.Tip = users[username].Tip;
            u.UserName = users[username].UserName;
            u.UserId = users[username].UserId;
            if (users[username].Image == null)
                u.Image = "a.jpg";
            else
                u.Image = users[username].Image;

            if (u.Active == true)
                active = "YES";
            else
                active = "NO";
            ViewBag.active = active;
            ViewBag.user = u;
            return View("Profile");
        }

        public ActionResult FacebookProfil()
        {
            string username = (string)Session["user"];
            User u = new User();
            u.Active = true; ;
            u.Address = (string)TempData["email"];
            u.DateOfBirth = DateTime.Now.Date;
            u.FirstName = (string)TempData["first_name"];
            u.LastName = (string)TempData["lastname"];
            u.Password = "";
            u.Tip = Models.User.TIP.CONSUMER;
            u.UserName = username;
            u.UserId = 666;
            u.Image = (string)TempData["picture"];
            u.Active = false;
            u.Verified = false;

          
            ViewBag.active = "NO";
            Session["activated"] = false;
            ViewBag.user = u;
            return View("Profile");

        }

        [HttpPost]
        public ActionResult Edit(string firstname, string lastname, string password, string address,string filename)
        {
            Dictionary<string, User> users = DataBase.LoadAllUsers();
            string username = (string)Session["user"];
            string active;
            User u = new User();

            u.Active = users[username].Active;
            if (string.IsNullOrEmpty(address))
            {
                u.Address = users[username].Address;
            }
            else
            {
                u.Address = address;
            }

            
            u.DateOfBirth = users[username].DateOfBirth;
            u.FirstName = firstname;
            u.LastName = lastname;
            if (string.IsNullOrEmpty(password))
            {
                u.Password = users[username].Password;
            }
            else
            {
                string pass = DataBase.sha256(password);
                u.Password = pass;
            }
            u.Tip = users[username].Tip;
            u.UserName = users[username].UserName;
            u.UserId = users[username].UserId;
            if (string.IsNullOrEmpty(filename))
            {
                u.Image = users[username].Image;
            }
            else
            {
                u.Image = "/Images/" + filename;
            }
           

            if (u.Active == true)
                active = "YES";
            else
                active = "NO";

            DataBase.UpdateUserToDataBase(u);
           
           

            ViewBag.active = active;
            ViewBag.user = u;
            return View("Profile");
        }

      
    }
}