using Facebook;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Restaurant.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            if (Session["activated"] == null)
                Session["activated"] = false;
           
            return View("Index");
        }

     

       public ActionResult LogOut()
        {
            Session["user"] = null;
            Session["type"] = null;
            return View("Index");
        }

        public ActionResult FacebookLogin()
        {
            Session["user"] = TempData["email"];
            Session["type"] = "CONSUMER";
            Session["activated"] = true;
            TempData["email"] = TempData["email"];
            TempData["first_name"] = TempData["first_name"];
            TempData["lastname"] = TempData["lastname"];
            TempData["picture"] = TempData["picture"];

            return RedirectToAction("FacebookProfil", "Profile");
        }

        [HttpPost]
        public ActionResult Login(string username,string password)
        {
            Dictionary<string, User> users;
                users = DataBase.LoadAllUsers();
                string pass = DataBase.sha256(password);

                foreach (var item in users.Values)
                {
                    if (item.UserName == username && item.Password == pass)
                    {
                        
                        Session["user"] = username;
                        Session["type"] = item.Tip.ToString();
                        Session["activated"] = item.Active;
                        HttpContext.Application.Add(username,users[username]);
                        return RedirectToAction("Index", "Profile");
                    }
                }
                ViewBag.ErrorMessage = "Incorrect username or password";
           
           
            return View("Index");
        }

        private Uri RediredtUri

        {

            get

            {

                var uriBuilder = new UriBuilder(Request.Url);

                uriBuilder.Query = null;

                uriBuilder.Fragment = null;

                uriBuilder.Path = Url.Action("FacebookCallback");

                return uriBuilder.Uri;

            }

        }




        [AllowAnonymous]
        public ActionResult Facebook()

        {

            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new

            {
                client_id = "350697397245961",

                client_secret = "812faac79ffbdea945cc0bd07e7aab4c",

                redirect_uri = RediredtUri.AbsoluteUri,

                response_type = "code",

                scope = "email"



            });

            return Redirect(loginUrl.AbsoluteUri);

        }




        public ActionResult FacebookCallback(string code)

        {

            var fb = new FacebookClient();

            dynamic result = fb.Post("oauth/access_token", new

            {

                client_id = "350697397245961",

                client_secret = "812faac79ffbdea945cc0bd07e7aab4c",

                redirect_uri = RediredtUri.AbsoluteUri,

                code = code




            });

            var accessToken = result.access_token;

            Session["AccessToken"] = accessToken;

            fb.AccessToken = accessToken;

            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

            string email = me.email;

            TempData["email"] = me.email;

            TempData["first_name"] = me.first_name;

            TempData["lastname"] = me.last_name;

            TempData["picture"] = me.picture.data.url;

            FormsAuthentication.SetAuthCookie(email, false);

            return RedirectToAction("FacebookLogin", "Home");

        }
    }
}