using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MikMak2016.Models;

namespace MikMak2016.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string name, string password)
        {
            if ("admin".Equals(name) && "123".Equals(password))
            {
                Session["user"] = new User();
                return RedirectToAction("Index", "Home");
            }
            else if ("user".Equals(name) && "321".Equals(password))
            {
                Session["user"] = new User();
                return RedirectToAction("IndexUser", "Home");
            }
            return View();


        }

        public ActionResult Logout()
        {
            //Session.Clear();
            Session.Abandon();
            // of Session["user"] = null;
            return RedirectToAction("Login", "Login"); ;
        }

    }
}
